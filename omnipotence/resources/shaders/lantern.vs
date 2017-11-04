#version 330 core
layout (location = 0) in vec3 l_position;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

void main() {
    gl_Position = u_projection * u_view * u_model * vec4(l_position, 1.0f);
}

