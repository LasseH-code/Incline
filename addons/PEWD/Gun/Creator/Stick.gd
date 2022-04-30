tool
extends Control

export(float) var speed:float = 1.0
export(float) var fine_speed:float = 3.0
export(float) var knob_speed:float = 2.0
export(bool) var relative_knob = false
export(bool) var do_z_scroll = false # Slightly buggy af
var mouse:bool = false
var m_rel:Vector2 = Vector2(0,0)
var s_rel:float = 0.0
var m_abs:Vector2 = Vector2(0,0)
var s_abs:float = 0.0
var half_sze:Vector2 = self.rect_min_size/2
var k_pos:Vector2#knob.rect_position

onready var knob = $Knob

signal relative_mov(mov)
signal absolute_mov(absolute)

func set_values(relative:Vector3, absolute:Vector3) -> void:
	m_rel = Vector2(relative.x, relative.y)
	s_rel = relative.z
	m_abs = Vector2(absolute.x, absolute.y)
	s_abs = absolute.z
	emit_sigs()

func reset() -> void:
	set_values(Vector3.ZERO, Vector3.ZERO)

func move_knob() -> void:
	if relative_knob:
		k_pos = knob.rect_position
	k_pos += m_rel
	#var a:Vector2 = Vector2(min(k_pos.x/2, self.rect_scale.x/2), min(k_pos.y/2, self.rect_scale.y/2))
	var a_sze:Vector2 = -half_sze+knob.rect_size
	var a:Vector2 = Vector2(clamp(k_pos.x, -a_sze.x , half_sze.x), clamp(k_pos.y, -a_sze.y, half_sze.y))
	knob.rect_position = a#+knob.rect_size/2
	#print("m_rel: ", m_rel, "; k_pos: ", k_pos, "; a: ", a)

func emit_sigs() -> void:
	emit_signal("relative_mov", Vector3(m_rel.x, m_rel.y, s_rel))
	m_abs+=m_rel
	emit_signal("absolute_mov", Vector3(m_abs.x, m_abs.y, s_abs))

func _on_Stick_gui_input(event):
	if mouse and event is InputEventMouseMotion:
		m_rel = event.relative * speed 
		if Input.is_key_pressed(KEY_SHIFT):
			m_rel /= fine_speed
		m_abs+=m_rel
		emit_sigs()
		move_knob()
	elif event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT:
			if event.pressed:
				mouse = true
				Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
				k_pos = knob.rect_position
			else:
				mouse = false
				Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
				get_viewport().warp_mouse(self.get_global_transform().origin+half_sze)
				knob.rect_position = knob.rect_size/2
				#emit_sigs()
		if mouse && do_z_scroll:
			if event.button_index == BUTTON_WHEEL_UP:
				if event.pressed:
					s_rel = speed
					s_abs += speed
					emit_sigs()
			elif event.button_index == BUTTON_WHEEL_DOWN:
				if event.pressed:
					s_rel = -speed
					s_abs -= speed
					emit_sigs()

