# 260220_command.md

## command 1
python, streamlitを使用して、ArduinoとPCのGUI間でJSON形式の通信を行うプログラムを作りたい。
- プログラムはGUI/python_streamlit以下に保存する
- PC側からArduino側にJSON形式のコマンドを送信する
- Arduino側は受信したJSON形式のコマンドを解析し、適切な処理を行う
- ArduinoのfirmwareはFirmware\hello_json_led\src\main.cppにある
- comポートの選択、ボーレートの指定をし、connectボタン、disconnectボタンがある
- connectしたあとに、LED ONボタンとLED OFFボタンがある
- LED ONボタンを押すと、Arduino側のLEDが点灯し、LED OFFボタンを押すと、Arduino側のLEDが消灯する
- comポートからの受信情報をGUIのテキストエリアに表示する

## command 2
現在のファイルを見て、README.mdに概略を追記してください。可能なら全体の構成図も追加してください

## command 3
README.mdを英語にしてください
