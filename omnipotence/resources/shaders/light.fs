#version 330 core

#define POINT_LIGHTS_NUMBER 4

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct DirectionalLight {
    vec3 direction;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
    vec3 position;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    //each of this values modify the point light differently
    //constant is independent of a distance between the light source and the item itself
    //the value of linear is multiplied by distance to get the result
    //the value of quadratic is multiplied by distance^2 to get the result
    float constant;
    float linear;
    float quadratic;
};

struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    //each of this values modify the point light differently
    //constant is independent of a distance between the light source and the item itself
    //the value of linear is multiplied by distance to get the result
    //the value of quadratic is multiplied by distance^2 to get the result
    float constant;
    float linear;
    float quadratic;
};

out vec4 color;

in vec3 fragPos;
in vec3 normal;
in vec2 texCoords;

uniform vec3 u_viewPos;
uniform Material u_material;
uniform DirectionalLight u_dirLight;
uniform SpotLight u_spotLight;
uniform PointLight u_pointLights[POINT_LIGHTS_NUMBER];
uniform bool u_textureLess;
uniform vec3 u_cubeColor;

vec3 calculateDirectionalLight(DirectionalLight light, vec3 normal, vec3 viewDirection);
vec3 calculatePointLight(PointLight light, vec3 normal, vec3 fragPosition, vec3 viewDirection);
vec3 calculateSpotLight(SpotLight light, vec3 normal, vec3 fragPosition, vec3 viewDirection);

void main() {
    vec3 norm = normalize(normal);
    vec3 viewDirection = normalize(u_viewPos - fragPos);
    
    vec3 result = calculateDirectionalLight(u_dirLight, norm, viewDirection);
    
    for (int i = 0; i < POINT_LIGHTS_NUMBER; i++) {
        result += calculatePointLight(u_pointLights[i], norm, fragPos, viewDirection);
    }
    
    result += calculateSpotLight(u_spotLight, norm, fragPos, viewDirection);
    
    color = vec4(result * u_cubeColor, 1.0);
    
}

vec3 calculateDirectionalLight(DirectionalLight light, vec3 normal, vec3 viewDirection) {
    vec3 lightDirection = normalize(-light.direction);           //it's minus because it's coming not from the object, but the light source
    
    //diffuse shading
    float diff = max(dot(normal, lightDirection), 0.0);
    
    //specular shading
    vec3 reflectDirection = reflect(-lightDirection, normal);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), u_material.shininess);

    //result
    vec3 ambient = light.ambient * vec3(texture(u_material.diffuse, texCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(u_material.diffuse, texCoords));
    vec3 specular = light.specular * spec * vec3(texture(u_material.specular, texCoords));
    
    return (ambient + diffuse + specular);
}
vec3 calculatePointLight(PointLight light, vec3 normal, vec3 fragPosition, vec3 viewDirection) {
    vec3 lightDirection = normalize(light.position - fragPos);           //it's minus because it's coming not from the object, but the light source
    float diff = max(dot(normal, lightDirection), 0.0);
    vec3 reflectDirection = reflect(-lightDirection, normal);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), u_material.shininess);
    float distance = length(light.position -fragPos);
    float attenuation = 1.0f / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    
    vec3 ambient = light.ambient * vec3(texture(u_material.diffuse, texCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(u_material.diffuse, texCoords));
    vec3 specular = light.specular * spec * vec3(texture(u_material.specular, texCoords));
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    
    return (ambient + diffuse + specular);
}
vec3 calculateSpotLight(SpotLight light, vec3 normal, vec3 fragPosition, vec3 viewDirection) {
    vec3 lightDirection = normalize(light.position - fragPos);           //it's minus because it's coming not from the object, but the light source
    float diff = max(dot(normal, lightDirection), 0.0);
    vec3 reflectDirection = reflect(-lightDirection, normal);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), u_material.shininess);
    float distance = length(light.position -fragPos);
    float attenuation = 1.0f / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    
    //spotlight intensity
    float theta = dot(lightDirection, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    
    vec3 ambient = light.ambient * vec3(texture(u_material.diffuse, texCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(u_material.diffuse, texCoords));
    vec3 specular = light.specular * spec * vec3(texture(u_material.specular, texCoords));
    
    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;
    
    return (ambient + diffuse + specular);
}
