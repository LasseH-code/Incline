tool
extends VBoxContainer

# Replace with a procedual property list at some point!
export(float) var speed:float = 0.3
export(float) var fine_speed:float = 3.0
export(float) var knob_speed:float = 2.0
export(bool) var relative_knob:bool = true
export(bool) var do_z_scroll:bool = false

onready var stick:Control = $HBoxContainer/Stick
onready var x_edit:LineEdit = $HBoxContainer/XYZ/X/XEdit
onready var y_edit:LineEdit = $HBoxContainer/XYZ/Y/YEdit
onready var z_edit:LineEdit = $HBoxContainer/XYZ/Z/ZEdit
onready var z_slider:HSlider = $"HBoxContainer2/Z Slider"

signal relative_mov(mov)
signal absolute_mov(absolute)

func _ready():
	stick.speed = speed
	stick.fine_speed = fine_speed
	stick.knob_speed = knob_speed
	stick.relative_knob = relative_knob
	stick.do_z_scroll = do_z_scroll

func update_labels(val):
	x_edit.text = str(val.x)
	y_edit.text = str(val.y)
	z_edit.text = str(val.z)

func _on_Stick_relative_mov(mov:Vector3):
	emit_signal("relative_mov", mov)

func _on_Stick_absolute_mov(absolute:Vector3):
	emit_signal("absolute_mov", absolute)
	update_labels(absolute)
	z_slider.value = absolute.z

func emit_stick_absolute() -> void:
	stick.emit_signal("absolute_mov", Vector3(stick.m_abs.x, stick.m_abs.y, stick.s_abs))

func _on_XEdit_text_entered(new_text:String):
	stick.m_abs.x = float(new_text)
	emit_stick_absolute()

func _on_YEdit_text_entered(new_text:String):
	stick.m_abs.y = float(new_text)
	emit_stick_absolute()

func _on_ZEdit_text_entered(new_text:String):
	var temp = float(new_text)
	stick.s_abs = temp
	z_slider.value = temp
	emit_stick_absolute()

func _on_Z_Slider_value_changed(value:float):
	stick.s_abs = value
	emit_stick_absolute()

func set_values(relative:Vector3, absolute:Vector3) -> void:
	stick.set_values(relative, absolute)

func reset() -> void:
	stick.reset()

func _on_Reset_button_up():
	reset()
