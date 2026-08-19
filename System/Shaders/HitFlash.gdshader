shader_type canvas_item;

// 0 = normal, 1 = flash
uniform float FlashModifier : hint_range(0.0, 1.0) = 0.0;
// flash color (white)
uniform vec4 FlashColor : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
    vec4 color = texture(TEXTURE, UV);
    // Mixing original sprite color based on FlashModifier
    color.rgb = mix(color.rgb, FlashColor.rgb, FlashModifier);
    COLOR = color;
}
