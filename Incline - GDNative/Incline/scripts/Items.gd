extends Node

var descriptions = {
	"Jumpboost" : "Allows you to jump heigher",
	"NULL": "does einfach nothing",
	"SPEED_UP": "Increases the player's speed",
	
}


var icons = {
	#"Jumpboost" : load("res://Feather-arrows-arrow-up.svg.png"),
	"NULL": load("res://logo.svg"),
	"SPEED_UP": null
	
}

func get_desc(type:String):
	return descriptions[type]

func get_icon(type:String):
	return icons[type]
