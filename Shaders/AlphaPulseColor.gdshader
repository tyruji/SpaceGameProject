shader_type spatial;

uniform vec3 color : source_color = vec3( 1 );
uniform float pulse_speed = 2;
uniform float min_alpha : hint_range(0.0, 1.0, 0.1) = .2;
uniform float max_alpha : hint_range(0.0, 1.0, 0.1) = .8;


void fragment() {
	ALBEDO = color;
	float transition = .5 * ( 1. + cos( TIME * pulse_speed ) );
	ALPHA = transition * ( min_alpha - max_alpha ) + max_alpha;
}
