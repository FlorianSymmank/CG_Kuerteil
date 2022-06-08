# version 330

out vec4 FragColor;
uniform vec4 instanceColor;

void main()
{
    FragColor = instanceColor;
}