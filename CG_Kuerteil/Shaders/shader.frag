﻿# version 330

out vec4 FragColor;
in vec3 myColor;

void main()
{
    FragColor = vec4(myColor, 1.0);
}