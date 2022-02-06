extends Spatial

onready var mesh = $Mesh
onready var anima = Anima.begin(mesh, "move")
onready var anim_end = $AnimEnd

export(String) var type = "NULL"
export(bool) var do_spin = true
export(float) var spin_speed = 1.0
export(bool) var animated:bool = true
export(float) var animation_time = 2.0

var animation_done = false

func create_move_anim():
	anima.then({node = mesh, property = "translation", from = mesh.translation, to = anim_end.translation, duration = animation_time, easing = Anima.EASING.EASE_IN_OUT})
	anima.then({node = mesh, property = "translation", from = anim_end.translation, to = mesh.translation, duration = animation_time, easing = Anima.EASING.EASE_IN_OUT})

func _ready():
	create_move_anim()
	if animated:
		_timer_callback()
		var timer = Timer.new()
		timer.set_one_shot(false)
		timer.set_wait_time(anima.get_length())
		timer.connect("timeout", self, "_timer_callback")
		timer.autostart = true
		add_child(timer)

func _timer_callback():
	anima.play()

func spin(delta) -> void:
	self.rotate_y(spin_speed*delta)

func _process(delta) -> void:
	if do_spin:
		spin(delta)


func _on_Pickuparea_body_entered(body):
	if body.is_in_group("Playable"):
		body.update_items(type)
		queue_free()
