#version 330 core

// There are four different types of variables in GLSL: input, output, uniform, and internal.
// - Input variables are sent from the buffer, in a way defined by GL.VertexAttribPointer.
// - Output variables are sent from this shader to the next one in the chain (which will be the fragment shader most of the time).
// - Uniforms will be touched on in the next tutorial.
// - Internal variables are defined in the shader file and only used there.

// The vertex shader is run once for every vertex

// This defines our input variable, aPosition.
// It starts with the line "layout(location = 0)". This defines where this input variable will be located, which is needed for GL.VertexAttribPointer.
// However, you can omit it, and replace this with just "in vec3 aPosition". If you do that, you'll have to replace the 0 in GL.VertexAttribPointer with
//   a call to GL.GetAttribLocation(shaderHandle, attributeName)
// Next, the keyword "in" defines this as an input variable.
// Then, the keyword "vec3" means this is a vector with 3 floats inside.

out vec3 Normal;
out vec3 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

in vec3 aPosition;
in vec3 aNormal;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = aNormal * mat3(transpose(inverse(model))); // nicht zu empfehlen, sehr kostenintensiv
}