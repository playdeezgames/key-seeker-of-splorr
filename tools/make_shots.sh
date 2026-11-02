#!/usr/bin/env bash
# Screenshots of the real build with headless Chrome (needs a built build/web and a free port 8124).
# Writes assets/screenshots/01..05. Each page plays its choices from the #play= hash (see web/index.html) on a fixed
# ?seed=, so the same world and the same dice come out every time. 06 to 08 are the original console version's.
set -euo pipefail
cd "$(dirname "$0")/.."
mkdir -p assets/screenshots
python3 -m http.server -d build/web 8124 >/dev/null 2>&1 &
srv=$!
trap 'kill $srv' EXIT
sleep 1
shot() { # name play
	google-chrome --headless=new --no-sandbox --disable-gpu --hide-scrollbars --window-size=800,600 \
		--virtual-time-budget=3000 --screenshot="assets/screenshots/$1.png" "http://localhost:8124/?seed=42#play=$2" >/dev/null 2>&1
}
START="c:Ok|c:Start|n:Jex"
shot 01-title ""
shot 02-town "$START"
shot 03-apothecary "$START|c:Move...|c:West|c:Move...|c:In|c:Interact...|c:Mårten the Nihilist Healer"
shot 04-fight "$START|c:Move...|c:West|c:Move...|c:West|c:Move...|c:East|c:Fight"
shot 05-status "$START|c:Status"
ls -l assets/screenshots
