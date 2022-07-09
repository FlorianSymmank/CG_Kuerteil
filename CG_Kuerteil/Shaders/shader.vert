#version 330 core

out vec3 Normal;
out vec3 FragPos;
out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

in vec3 aPosition;
in vec3 aNormal;
in vec2 aTexCoord;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = aNormal * mat3(transpose(inverse(model))); // nicht zu empfehlen, sehr kostenintensiv
    texCoord = aTexCoord;
}