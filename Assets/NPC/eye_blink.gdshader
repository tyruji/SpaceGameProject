shader_type spatial;

uniform vec3 blink_color : source_color = vec3( 0, 0, 0 );
uniform float blink_period = 2;
uniform float blink_length = .2;

float blinkAlpha( float time )
{
	time = mod( time, blink_period );

	float curve = time - blink_period + 2. * blink_length;
	curve *= time - blink_period;
	curve *= - 1.0 / ( blink_length * blink_length );

	return max( 0, curve );
}
void vertex()
{
	VERTEX.y *= blinkAlpha( TIME );
}
void fragment() {
	ALBEDO = blink_color;
}