shader_type sky;

uniform vec3 skyColor : source_color;
uniform sampler2D skyTexture;

void sky() {
	vec3 sky = texture(skyTexture, vec2(SKY_COORDS.x + TIME / 1000.0, SKY_COORDS.y)).rgb;
	vec3 finalColor = mix(sky, skyColor, vec3(0.1,0.2,0.4));
	COLOR = finalColor;
}
