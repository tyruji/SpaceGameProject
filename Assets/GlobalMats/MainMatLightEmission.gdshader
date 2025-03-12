shader_type spatial;

uniform vec4 emission_color : source_color = vec4( 1 );
uniform vec4 color_to_replace : source_color = vec4( 1 );

void fragment() {
	float r = COLOR.r - color_to_replace.r;
	float g = COLOR.g - color_to_replace.g;
	float b = COLOR.b - color_to_replace.b;

	ALPHA = step( -0.1, r + g + b );

	EMISSION = emission_color.rgb;
}
