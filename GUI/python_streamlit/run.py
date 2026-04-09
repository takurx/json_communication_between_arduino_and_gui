import streamlit
from threading import Thread
import streamlit.components.v1 as components

import streamlit.web.cli as stcli
import os
import sys

import serial
import serial.tools.list_ports
import json
import threading
import time

def resolve_path(path):
    resolved_path = os.path.abspath(os.path.join(os.getcwd(), path))
    return resolved_path

if __name__ == "__main__":
    sys.argv = [
        "streamlit",
        "run",
        resolve_path("app.py"),
        "--global.developmentMode=false",
    ]
    sys.exit(stcli.main())
