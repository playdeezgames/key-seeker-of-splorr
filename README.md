# Key Seeker of SPLORR!!

ASCII Game Jam 2023 (theme: Machine), by TheGrumpyGameDev. A turn-based text RPG built entirely from maze generators.

## The Keys

- [x] To the Sewer, reward from the Innkeeper for killing rats.
- [x] To the Graveyard, found in the Sewer.
- [x] To the Tower, found in the Graveyard.
- [x] To the UFO, found in the Tower.
- [x] To the Machine in the Ruins, found in the UFO.

## Playing

- Itch.io: https://thegrumpygamedev.itch.io/key-seeker-of-splorr (the browser build, plus the original Windows, Linux and macOS downloads).
- GitHub Pages: https://playdeezgames.github.io/key-seeker-of-splorr/ (`tools/pages.sh`, commit `docs/`, push).

## The Odin port (November 2026)

The game was rewritten from VB.NET and Spectre.Console to Odin, compiled to WebAssembly (`js_wasm32`), with Claude Code (Anthropic's AI coding assistant) at the author's direction. The rules, numbers, shops and mazes are the original's; `QUIRKS.md` lists the oddities kept on purpose and the few differences.

- `odin/game`: the pure rules and the menus (no platform imports), with its tests.
- `odin/platform/web` and `web/index.html`: the thin browser layer (drawing the menus, local storage).
- `tools/test.sh` runs the tests natively and type-checks for wasm; `tools/build.sh [--size]` builds into `build/web`; `tools/serve.sh` serves it; `tools/ship.sh` prepares the itch.io build (pushes only with `--push`); `tools/pages.sh` builds `docs/` for GitHub Pages.
- Needs [Odin](https://odin-lang.org/) (developed on `dev-2026-07`).

The original VB.NET source is still under `src/` and the old `pub-*` folders hold its published builds; they will be removed from the tree once the browser build is live (they stay in git history).

Licence: MIT.
