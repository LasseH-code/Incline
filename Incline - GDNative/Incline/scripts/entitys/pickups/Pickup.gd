extends Spatial

onready var meshNode = $MeshNode
onready var anima = Anima.begin(meshNode, "move")
onready var anim_end = $AnimEnd
onready var payload = $Payloads
export(String) var type = "NULL"
export(bool) var do_spin = true
export(float) var spin_speed = 1.0
export(bool) var animated:bool = true
export(float) var animation_time = 2.0
export(Mesh) var meshOverride

var animation_done = false

onready var seperatePayloads = get_payloads()

func override_mesh() -> void:
	meshNode.mesh = meshOverride

func get_payloads():
	var p = payload.get_children()
	#if p.size() <= 0:
	#	var temp = Spatial.new()
	#	temp.name == "NULL"
	#	payload.add_child(temp)
	#	p = payload.get_children()
	return p

func create_move_anim() -> void:
	anima.then({node = meshNode, property = "translation", from = meshNode.translation, to = anim_end.translation, duration = animation_time, easing = Anima.EASING.EASE_IN_OUT})
	anima.then({node = meshNode, property = "translation", from = anim_end.translation, to = meshNode.translation, duration = animation_time, easing = Anima.EASING.EASE_IN_OUT})

func _ready() -> void:
	if animated:
		create_move_anim()
		_timer_callback()
		var timer = Timer.new()
		timer.set_one_shot(false)
		timer.set_wait_time(anima.get_length())
		timer.connect("timeout", self, "_timer_callback")
		timer.autostart = true
		add_child(timer)

func _timer_callback() -> void:
	anima.play()

func spin(delta) -> void:
	self.rotate_y(spin_speed*delta)

func _process(delta) -> void:
	if do_spin:
		spin(delta)


func _on_Pickuparea_body_entered(body) -> void:
	if body.is_in_group("Playable"):
		#seperatePayloads = get_payloads()
		body.recieve_item(type, seperatePayloads)
		queue_free()
