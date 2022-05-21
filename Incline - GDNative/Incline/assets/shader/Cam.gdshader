shader_type canvas_item;

const float PI = 3.14159;

uniform vec2 middle = vec2(.5,.5);

uniform bool do_chromatic_abberation = true;
uniform vec2 offset_r = vec2(3,3);
uniform vec2 offset_g = vec2(1,1);
uniform vec2 offset_b = vec2(-.2,-.2);
uniform vec2 abberation_strength = vec2(.004f,.004f);

uniform bool do_distortion = true;
uniform float distortion_strength = -0.4;
uniform float distortion_scale = 1.75;

uniform bool do_vignette = false;
uniform float vignette_multiplier = 1;
uniform float vignette_scale = 1.159;
uniform float vignette_softness = 1.159;
uniform vec4 vignette_color : hint_color = vec4(0, 0, 0, 1);

float Q_isqrt( float number )
{
	highp int i;
	float x2, y;
	const float threehalfs = 1.5F;

	x2 = number * 0.5F;
	y  = number;
	i  = floatBitsToInt(y);                     // evil floating point bit level hacking
	i  = 0x5f3759df - ( i >> 1 );               // what the fuck? 
	y  = intBitsToFloat(i);
	y  = y * ( threehalfs - ( x2 * y * y ) );   // 1st iteration
//	y  = y * ( threehalfs - ( x2 * y * y ) );   // 2nd iteration, this can be removed

	return y;
}
float Q_sqrt(float number)
{
	return 1f/Q_isqrt(number);
}

vec2 delta_btw_points_sqr(vec2 a, vec2 b)
{
	return vec2((a.x-b.x), (a.y-b.y));
}
float dist_btw_points_sqr_f(vec2 a, vec2 b)
{
	vec2 dst = delta_btw_points_sqr(a,b);
	dst = vec2(dst.x*dst.x,dst.y*dst.y);
	return dst.x+dst.y;
}

vec4 vignette(vec2 uv, vec4 col)
{
	float val = distance(vec2(uv.x, uv.y * vignette_multiplier), middle);
	val /= vignette_scale;
	float vig = smoothstep(0.2, vignette_softness, val);
	return mix(col, vignette_color, vig);
	/*col = mix(col, vignette_color, dist_btw_points_sqr_f(uv, middle)-vignette_threshold);
	return col;*/
}

vec3 radial_chromatic_abberation(sampler2D screen, vec2 uv, vec2 dist)
{
	vec2 adjusted_str = abberation_strength*dist;
	float col_r = texture(screen, uv+(offset_r*adjusted_str)).r;
	float col_g = texture(screen, uv+(offset_g*adjusted_str)).g;
	float col_b = texture(screen, uv+(offset_b*adjusted_str)).b;
	return vec3(col_r, col_g, col_b);
}

vec2 distort_uv(vec2 uv)
{
	float d = length(uv);
	float z = Q_sqrt(1.0+d*d*distortion_strength);
	float r = atan(d,z) / PI;
	r *= distortion_scale;
	float phi = atan(uv.y, uv.x);
	
	return vec2(r*cos(phi)+.5, r*sin(phi)+.5);
}

void fragment()
{
	vec2 uv;
	if (do_distortion)
	{
		uv = distort_uv((SCREEN_UV-middle)*2f);
	}
	else 
	{
		uv = SCREEN_UV;
	}
	
	vec2 dist = delta_btw_points_sqr(uv, middle);
	vec4 col = texture(SCREEN_TEXTURE, uv);
	if (do_chromatic_abberation) col.rgb = radial_chromatic_abberation(SCREEN_TEXTURE, uv, dist);
	if (do_vignette) col = vignette(uv, col);
	COLOR = col;
}