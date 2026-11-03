# CLAUDE.md

Key Seeker of SPLORR!! (ASCII Game Jam 2023). The VB.NET original (`src/`, `pub-*`, the `abettor-of-splorr` submodule) was removed from the tree on November 3, 2026 and lives in git history only; the Odin/wasm game is `odin/game` (pure rules and menus), `odin/platform/web` and `web/index.html`. Context lives in the vault: `bok-of-splorr/splorr/Games/Key Seeker of SPLORR!!.md`.

- Never `git push` unless the user says so. "Push" is `git push`; "ship" is itch.io (`tools/ship.sh --push`, or `shippit.sh --push`). Do not run either unless told, and check Coxswain's `ship_today` first (one ship a day).
- Read `QUIRKS.md` before "fixing" anything odd, and add new oddities there. The original's behaviour is deliberate until the user says otherwise.
- Tests: `tools/test.sh` (about 90 seconds; most of it is compiling). `int` is 32 bits on `js_wasm32`: do random-number modulo in `u64` first.
- The itch page already has windows, linux and mac downloads; the browser build is the `html` channel. Do not rename channels of the live page.
- GitHub Pages: live at https://playdeezgames.github.io/key-seeker-of-splorr/ (branch main, /docs; update with `tools/pages.sh`, commit, push).
