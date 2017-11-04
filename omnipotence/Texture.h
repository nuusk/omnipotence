#pragma once

#define GLEW_STATIC
#include <GL/glew.h>
#include <vector>

class Texture {
public:
    static GLuint assignTexture(GLchar *path) {

    GLuint textureID;
    int textureWidth, textureHeight;
    unsigned char *image;
    
    //texture generating and binding
    glGenTextures(1, &textureID);
    glBindTexture(GL_TEXTURE_2D, textureID);
    
    //texture parameters
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    
    //texture filtering
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST_MIPMAP_NEAREST);
    
    //load texture image
    image = SOIL_load_image(path, &textureWidth, &textureHeight, 0, SOIL_LOAD_RGB);
    
    //generate mipmaps
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, textureWidth, textureHeight, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
    glGenerateMipmap(GL_TEXTURE_2D);
    
    //free image and unbind the texture
    SOIL_free_image_data(image);
    glBindTexture(GL_TEXTURE_2D, 0);
    
    return textureID;
    }
    
    static GLuint assignSkybox(std::vector<const GLchar *> faces) {
    
        GLuint textureID;
        int textureWidth, textureHeight;
        unsigned char *image;
        
        //texture generating and binding
        glGenTextures(1, &textureID);
        glBindTexture(GL_TEXTURE_CUBE_MAP, textureID);
        
        for (GLuint i = 0; i < faces.size(); i++) {
            //load texture image
            image = SOIL_load_image(faces[i], &textureWidth, &textureHeight, 0, SOIL_LOAD_RGB);
            
            //           //flag to tell we're using skybox (+i for every cube wall)
            glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_X + i, 0, GL_RGB, textureWidth, textureHeight, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
            SOIL_free_image_data(image);
        }
        
        //texture parameters
        glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
        glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
        glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE);
        
        //texture filtering
        glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
        
        glBindTexture(GL_TEXTURE_CUBE_MAP, 0);
        
        return textureID;
    }
};
