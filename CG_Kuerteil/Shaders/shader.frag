# version 330

out vec4 FragColor;

uniform vec4 objectColor;
uniform vec4 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;

in vec3 Normal;
in vec3 FragPos;

void main()
{
    // Ambient Lighting
    float ambientStrength = 0.1;
    vec4 ambient = ambientStrength * lightColor;

    // Diffuse Lighting
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);  

    float diff = max(dot(norm, lightDir), 0.0); // dot(norm, lightDir), komponentenweise multiplikation, ergebniss komponenten addition
    vec4 diffuse = diff * lightColor;

    // Specular Lighting
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32); //The 32 is the shininess of the material.
    vec4 specular = specularStrength * spec * lightColor;

    vec4 result = (ambient + diffuse + specular) * objectColor;
    FragColor = result;
}