extends Control

var inout_anim := Anima.begin(self, "inout")
var zoomy_fuck := Anima.begin(self, "zoomy_fuck")

var mm = preload("res://_sc/menus/MainMenu.tscn")

func _input(event) -> void:
	if event is InputEventKey and event.is_pressed():
		change_to_main_menu()

func _ready():
	#Engine.set_
	$Inertia.modulate.a = 0.0
	$Incline.modulate.a = 0.0
	make_inout_anim()
	make_zoomy_anim()
	inout_anim.play()
	zoomy_fuck.play_with_delay(inout_anim.get_length())
	var timer = Timer.new()
	timer.set_one_shot(true)
	timer.set_wait_time(zoomy_fuck.get_length()+inout_anim.get_length()+0.5)
	timer.connect("timeout", self, "_timer_callback")
	timer.autostart = true
	#timer.start()
	add_child(timer)
	

func change_to_main_menu():
	get_tree().change_scene_to(mm)

func _timer_callback():
	change_to_main_menu()

func make_zoomy_anim():
	zoomy_fuck.then({node = $Incline, animation = "fade_in", duration = 1.0})
	zoomy_fuck.then({node = $Incline, property = "rotation_degrees", from = $Incline.rotation_degrees, to = $Incline2.rotation_degrees, duration = 2.0, easing = Anima.EASING.EASE_IN_OUT, delay = 1.0})
	zoomy_fuck.with({node = $Incline, property = "position", from = $Incline.position, to = $Incline2.position, duration = 2.0, easing = Anima.EASING.EASE_IN_OUT})
	#zoomy_fuck.with({node = $Incline, property = "scale", from = $Incline.scale, to = $Incline.scale - Vector2(0.1, 0.1), duration = 0.3})
	zoomy_fuck.with({node = $Incline, property = "scale", from = $Incline.scale, to = $Incline2.scale, duration = 2.0, delay = 0.0, easing = Anima.EASING.EASE_IN_OUT})

func make_inout_anim():
	inout_anim.then({node = $Inertia, animation = "fade_in", duration = 1.0})
	inout_anim.then({node = $Inertia, animation = "fade_out", duration = 1.0, delay = 0.7})
