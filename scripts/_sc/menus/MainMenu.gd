extends Control

var splash_fade_out := Anima.begin(self, "spash_fade_out")
var options_slide_in := Anima.begin(self, "options_slide_in")

onready var options = $CanvasLayer2/Options

func _ready():
	$CanvasLayer2/Splash/SplashFade.visible = true
	make_splash_fade_out_anim()
	make_options_slide_in_anim()
	splash_fade_out.play()

func make_options_slide_in_anim() -> void:
	options_slide_in.then({node = options, property = "position", from = options.position, to = $CanvasLayer2/OptionsEnd.position, duration = 2.2, easing = Anima.EASING.EASE_IN_OUT, delay = 0.3})

func make_splash_fade_out_anim() -> void:
	splash_fade_out.then({node = $CanvasLayer2/Splash/SplashFade, animation = "fade_out", duration = 2.0, delay = 0.5})

func _on_Button_pressed() -> void:
	get_tree().change_scene("res://_sc/tests/PlayerTest.tscn")

func _on_Options_toggled(button_pressed) -> void:
	if button_pressed:
		options_slide_in.play()
	else:
		options_slide_in.play_backwards()
