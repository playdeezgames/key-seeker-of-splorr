#!/usr/bin/env bash
# Native tests of the pure game package, plus a wasm type-check of every package. Usage: tools/test.sh
set -euo pipefail
cd "$(dirname "$0")/.."
odin test odin/game -o:speed -define:ODIN_TEST_THREADS=1
odin check odin/game -target:js_wasm32 -no-entry-point -vet-shadowing -vet-unused
odin check odin/platform/web -collection:ksos=odin -target:js_wasm32 -vet-shadowing -vet-unused
echo "ALL TESTS PASSED"
