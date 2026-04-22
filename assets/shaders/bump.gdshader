shader_type spatial;

uniform sampler2D tex : source_color;
uniform sampler2D bump_map;
uniform sampler2D normal_map : hint_normal;
uniform float height_scale = 1.0;
uniform float width_x = 10.f;
uniform float width_z = 10.f;



varying vec2 tex_position;
void vertex() {
	// Called for every vertex the material is visible on.
	tex_position = vec2(VERTEX.x / width_x,VERTEX.z / width_z) + 0.5;
	float height = texture(bump_map,tex_position).x * height_scale;
	VERTEX.y += height;
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO = texture(tex,tex_position).xyz;
	NORMAL_MAP *= texture(normal_map, tex_position).xyz;
}

void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
    DIFFUSE_LIGHT += clamp(dot(NORMAL, LIGHT), 0.0, 1.0) * ATTENUATION * LIGHT_COLOR / PI;
}
