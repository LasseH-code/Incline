extends Node

var descriptions = {
	"Jumpboost" : "Allows you to jump heigher",
	"NULL": "does einfach nothing",
	"SPEED_UP": "Increases the player's speed",
	"TEST_ABILITY": "Adds a big grey box"
}


var icons = {
	#"Jumpboost" : load("res://Feather-arrows-arrow-up.svg.png"),
	"NULL": load("res://logo.svg"),
	"SPEED_UP": null,
	"TEST_ABILITY": null
}

func get_desc(type:String):
	return descriptions[type]

func get_icon(type:String):
	return icons[type]
