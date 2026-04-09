"""
app.py - Arduino JSON LED Control GUI using Streamlit

ArduinoとStreamlit GUIの間でJSON形式の通信を行うプログラム。
- COMポートの選択・ボーレートの指定・接続/切断
- LED ON/OFFコマンドをJSON形式でArduinoに送信
- Arduinoからの受信データをテキストエリアに表示

Arduino firmware: Firmware/hello_json_led/src/main.cpp
  受信コマンド: {"cmd": "power", "value": 1}  // LED ON
              {"cmd": "power", "value": 0}  // LED OFF
  送信データ:  {"led_state": true/false}     // 1秒ごとにArduinoから送信
"""

import streamlit as st
import serial
import serial.tools.list_ports
import json
import threading
import time

# ─────────────────────────────────────────────
# ページ設定
# ─────────────────────────────────────────────
st.set_page_config(
    page_title="Arduino JSON LED Control",
    page_icon="💡",
    layout="wide",
)

st.title("💡 Arduino JSON LED Control")
st.caption("Arduino UNO R4 WiFi との JSON 通信 GUI")

# ─────────────────────────────────────────────
# セッション状態の初期化
# ─────────────────────────────────────────────
defaults = {
    "serial_conn": None,
    "connected": False,
    "received_data": [],   # 受信ログ（文字列のリスト）
    "read_thread": None,
    "stop_event": None,
    "led_state": None,     # Arduinoから受け取ったLED状態
}
for key, val in defaults.items():
    if key not in st.session_state:
        st.session_state[key] = val


# ─────────────────────────────────────────────
# ユーティリティ関数
# ─────────────────────────────────────────────
def get_com_ports() -> list[str]:
    """利用可能なCOMポート一覧を返す"""
    ports = serial.tools.list_ports.comports()
    return [p.device for p in sorted(ports)]


def append_log(message: str):
    """受信ログにメッセージを追加（最大200行）"""
    ts = time.strftime("%H:%M:%S")
    st.session_state.received_data.append(f"[{ts}] {message}")
    if len(st.session_state.received_data) > 200:
        st.session_state.received_data = st.session_state.received_data[-200:]


def serial_reader(ser: serial.Serial, stop_event: threading.Event):
    """
    バックグラウンドスレッドでシリアルデータを読み取る。
    受信した行はセッション状態の received_data に追記する。
    JSON形式の場合は led_state も更新する。
    """
    while not stop_event.is_set():
        try:
            if ser.in_waiting > 0:
                raw = ser.readline()
                line = raw.decode("utf-8", errors="replace").strip()
                if line:
                    append_log(f"RX: {line}")
                    # JSON解析してled_stateを更新
                    try:
                        data = json.loads(line)
                        if "led_state" in data:
                            st.session_state.led_state = data["led_state"]
                    except json.JSONDecodeError:
                        pass
        except serial.SerialException as e:
            if not stop_event.is_set():
                append_log(f"[ERROR] Serial error: {e}")
            break
        except Exception as e:
            if not stop_event.is_set():
                append_log(f"[ERROR] {e}")
            break
        time.sleep(0.02)


def send_json_command(payload: dict):
    """JSON形式のコマンドをArduinoへ送信する"""
    ser: serial.Serial = st.session_state.serial_conn
    if ser is None or not ser.is_open:
        st.error("シリアルポートが開いていません。")
        return
    cmd_str = json.dumps(payload, separators=(",", ":"))
    ser.write((cmd_str + "\n").encode("utf-8"))
    append_log(f"TX: {cmd_str}")


def do_connect(port: str, baud: int):
    """シリアルポートへ接続し、読み取りスレッドを開始する"""
    ser = serial.Serial(port, baud, timeout=1)
    st.session_state.serial_conn = ser
    st.session_state.connected = True
    st.session_state.received_data = []
    st.session_state.led_state = None

    stop_event = threading.Event()
    st.session_state.stop_event = stop_event
    t = threading.Thread(
        target=serial_reader,
        args=(ser, stop_event),
        daemon=True,
    )
    t.start()
    st.session_state.read_thread = t
    append_log(f"Connected to {port} at {baud} bps")


def do_disconnect():
    """シリアルポートを切断し、読み取りスレッドを停止する"""
    if st.session_state.stop_event:
        st.session_state.stop_event.set()
    time.sleep(0.15)
    if st.session_state.serial_conn and st.session_state.serial_conn.is_open:
        st.session_state.serial_conn.close()
    st.session_state.serial_conn = None
    st.session_state.connected = False
    st.session_state.read_thread = None
    st.session_state.stop_event = None
    append_log("Disconnected")


# ─────────────────────────────────────────────
# サイドバー: 接続設定
# ─────────────────────────────────────────────
with st.sidebar:
    st.header("🔌 接続設定")

    # COMポート
    available_ports = get_com_ports()
    if available_ports:
        selected_port = st.selectbox(
            "COMポート",
            options=available_ports,
            disabled=st.session_state.connected,
        )
    else:
        st.warning("利用可能なCOMポートが見つかりません。")
        selected_port = None

    # ボーレート
    baud_options = [9600, 19200, 38400, 57600, 115200]
    selected_baud = st.selectbox(
        "ボーレート",
        options=baud_options,
        index=baud_options.index(115200),
        disabled=st.session_state.connected,
    )

    st.divider()

    # 接続状態インジケータ
    if st.session_state.connected:
        st.success("● 接続中")
    else:
        st.error("○ 未接続")

    col_con, col_dis = st.columns(2)

    with col_con:
        connect_clicked = st.button(
            "Connect",
            disabled=st.session_state.connected or selected_port is None,
            use_container_width=True,
        )

    with col_dis:
        disconnect_clicked = st.button(
            "Disconnect",
            disabled=not st.session_state.connected,
            use_container_width=True,
        )

    if connect_clicked and selected_port:
        try:
            do_connect(selected_port, selected_baud)
            st.rerun()
        except serial.SerialException as e:
            st.error(f"接続失敗: {e}")

    if disconnect_clicked:
        do_disconnect()
        st.rerun()


# ─────────────────────────────────────────────
# メインエリア: LED制御
# ─────────────────────────────────────────────
st.header("🔴 LED 制御")

if st.session_state.connected:
    # Arduinoから受け取ったLED状態を表示
    if st.session_state.led_state is True:
        st.info("Arduino LED 状態: **ON** 💡")
    elif st.session_state.led_state is False:
        st.info("Arduino LED 状態: **OFF** 🔕")
    else:
        st.info("Arduino LED 状態: 取得中...")

    col_on, col_off = st.columns(2)

    with col_on:
        if st.button(
            "💡 LED ON",
            use_container_width=True,
            type="primary",
        ):
            send_json_command({"cmd": "power", "value": 1})

    with col_off:
        if st.button(
            "🔴 LED OFF",
            use_container_width=True,
            type="secondary",
        ):
            send_json_command({"cmd": "power", "value": 0})
else:
    st.info("Arduino に接続してから LED を制御できます。")


# ─────────────────────────────────────────────
# メインエリア: シリアルモニタ
# ─────────────────────────────────────────────
st.header("📋 シリアルモニタ")

ctrl_col1, ctrl_col2, ctrl_col3 = st.columns([1, 1, 4])

with ctrl_col1:
    if st.button("🔄 更新", use_container_width=True):
        st.rerun()

with ctrl_col2:
    if st.button("🗑️ クリア", use_container_width=True):
        st.session_state.received_data = []
        st.rerun()

# テキストエリアに受信ログを表示（新しいメッセージが下に来るよう逆順表示）
log_text = (
    "\n".join(st.session_state.received_data)
    if st.session_state.received_data
    else "（受信データなし）"
)

st.text_area(
    label="受信ログ",
    value=log_text,
    height=350,
    disabled=True,
    label_visibility="collapsed",
)

# ─────────────────────────────────────────────
# 接続中は自動更新（0.5秒ごとにリフレッシュ）
# ─────────────────────────────────────────────
if st.session_state.connected:
    time.sleep(0.5)
    st.rerun()
