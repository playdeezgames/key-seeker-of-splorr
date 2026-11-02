#+build js
package main

import "base:runtime"
import "ksos:game"

foreign import host "env"

@(default_calling_convention = "contextless")
foreign host {
	// The page is rebuilt from a description each time: clear it, add the parts, end it.
	dom_clear :: proc() ---
	dom_heading :: proc(text: [^]u8, length: i32) ---
	dom_line :: proc(text: [^]u8, length: i32) ---
	dom_title :: proc(text: [^]u8, length: i32) ---
	dom_choice :: proc(text: [^]u8, length: i32) ---
	dom_text_prompt :: proc(initial: [^]u8, length: i32) ---
	dom_end :: proc() ---
	// Copies the text box into buf (at most cap bytes) and returns its length.
	read_text :: proc(buf: [^]u8, cap: i32) -> i32 ---
	// Milliseconds since the epoch, as f64 (an i64 import would need a BigInt on the JS side).
	now_ms :: proc() -> f64 ---
	// localStorage. storage_get returns the value's byte length, or -1 when the key is missing, unreadable or
	// longer than cap. storage_set returns 1 when the value was stored.
	storage_get :: proc(key: [^]u8, key_len: i32, buf: [^]u8, cap: i32) -> i32 ---
	storage_set :: proc(key: [^]u8, key_len: i32, val: [^]u8, val_len: i32) -> i32 ---
	storage_remove :: proc(key: [^]u8, key_len: i32) ---
}

// raw_data rejects a constant string, so the key is a variable.
save_key := "ksos-save"

state: ^game.Game
save_buf: [1 << 19]u8
input_buf: [256]u8

web_write :: proc(text: string) -> bool {
	return storage_set(raw_data(save_key), i32(len(save_key)), raw_data(text), i32(len(text))) == 1
}

web_read :: proc() -> (string, bool) {
	n := storage_get(raw_data(save_key), i32(len(save_key)), raw_data(save_buf[:]), i32(len(save_buf)))
	if n < 0 {
		return "", false
	}
	return string(save_buf[:n]), true
}

web_clear :: proc() {
	storage_remove(raw_data(save_key), i32(len(save_key)))
}

// Draws whatever screen the game is on.
render :: proc() {
	v := game.make_view(state)
	dom_clear()
	if len(v.heading) > 0 {
		dom_heading(raw_data(v.heading), i32(len(v.heading)))
	}
	for line in v.lines {
		dom_line(raw_data(line), i32(len(line)))
	}
	if len(v.title) > 0 {
		dom_title(raw_data(v.title), i32(len(v.title)))
	}
	if v.text {
		dom_text_prompt(raw_data(v.initial), i32(len(v.initial)))
	}
	for c in v.choices {
		dom_choice(raw_data(c), i32(len(c)))
	}
	dom_end()
	free_all(context.temp_allocator)
}

// Every answer ends the same way: the game saves itself if it is at a quiet point, then the page is redrawn.
settle :: proc() {
	game.settle(state)
	render()
}

main :: proc() {
	state = game.new_game(u64(now_ms()))
	game.storage = game.Storage {
		write = web_write,
		read  = web_read,
		clear = web_clear,
	}
	// a game left in progress is waiting behind the title screen as "Continue"
	if game.load_saved(state) {
		state.rng.state ~= u64(now_ms())
		state.screen = .Title
	}
	render()
}

// odin.js stops after main unless a step export exists; the page is event driven, so it does nothing.
@(export)
step :: proc(dt: f64, c: runtime.Context) -> bool {
	return true
}

@(export)
on_choice :: proc "c" (index: i32) {
	context = runtime.default_context()
	game.choose(state, int(index))
	settle()
}

@(export)
on_submit :: proc "c" () {
	context = runtime.default_context()
	n := read_text(raw_data(input_buf[:]), i32(len(input_buf)))
	game.submit_text(state, string(input_buf[:n]))
	settle()
}
