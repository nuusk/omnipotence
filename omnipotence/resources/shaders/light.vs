#version 330 core
layout (location = 0) in vec3 l_position;
layout (location = 1) in vec3 l_normal;
layout (location = 2) in vec2 l_texCoords;

out vec3 normal;
out vec3 fragPos;
out vec2 texCoords;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

void main() {
    gl_Position = u_projection * u_view * u_model * vec4(l_position, 1.0f);
    fragPos = vec3(u_model * vec4(l_position, 1.0f));
    normal = mat3(transpose(inverse(u_model))) * l_normal;
    texCoords = l_texCoords;
}
