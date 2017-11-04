#version 330 core
layout (location = 0) in vec3 l_position;

out vec3 texCoords;

uniform mat4 u_view;
uniform mat4 u_projection;

void main() {
    vec4 pos = u_projection * u_view * vec4(l_position, 1.0);
    gl_Position = pos.xyww;
    texCoords = l_position;
}

