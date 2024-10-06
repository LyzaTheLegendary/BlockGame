#version 330 core
layout (location = 0) in vec3 aPosition; // vertex coords
layout (location = 1) in vec2 aTexCoord; // texture coords 
// move object location here?

out vec2 texCoord;

// uniform variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
	gl_Position = vec4(aPosition, 1.0f) * model * view * projection;
	texCoord = aTexCoord;
}