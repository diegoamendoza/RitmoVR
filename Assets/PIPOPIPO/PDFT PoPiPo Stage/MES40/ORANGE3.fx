//Customizable settings for MES40

//Custom settings per object (Apply this fx to the object)

//The #define lines either take 0 (disabled) or 1 (enabled) as the value, or a texture's name

//The float lines can take any decimal number, but not all of them will produce nice visual

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//NOTICE: If you use the HgShadow version of the shader, then ignore this section. Go to the "HgShadow_ViewportMap.fxsub" in the "HgShadow Custom FOV" folder, these settings will be repeated at the beginning of the file, edit them there, not here. Finally, apply that fxsub on the model in the "HgS_VMap" tab

#define CUSTOM_FOV 0

	#define Custom_Fov_Model "MES40 Controller.pmd"
	#define Custom_Fov_Bone "fov"
	#define Custom_Fov_Axis 1 // 1=x | 2=y | 3=z ; Determine which direction of the bone controls the FOV value

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//#define AnimatedTexture "animated.gif"

#define NormalMapTexture  "F_DIVA_STGD2NS057_NK_ORANGE_NML.png"

	#define Toon_Use_NormalMap 1
	#define Spa_CubeMap_Use_NormalMap 1
	#define SpecularLight_Use_NormalMap 1
	
	float NormalMap_Intensity = 1;
	#define NormalMap_Animated 0
	
#define SpecularMapTexture  "F_DIVA_STGD2NS057_NK_ORANGE_SP.png"

	#define Spa_CubeMap_Use_SpecularMap 1
	#define SpecularLight_Use_SpecularMap 1
	
	float SpecularMap_Saturation = 1;
	#define SpecularMap_Animated 0

#define CubeMapTexture "F_DIVA_STGD2NS057_NK.dds"

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//See custom_basic.png for details, leave these things alone if you want to keep the same setting as MMD's default shader

float Toon_Intensity = 1;
float Toon_Brightness = 1;
float Toon_Gradient = 3;

float Spa_CubeMap_Intensity = 0.5;
float Spa_CubeMap_Saturation = 1;
#define CubeMap_Affected_By_LightDirection 0

float SpecularLight_Intensity = 0.5;
float SpecularLight_Focus = 5;
#define SpecularLight_Affected_By_LightDirection 1

float Spa_CubeMap_SpecularLight_Tint = 1;


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//See custom_rim.png for details

float3 Rim_Color = float3(1,1,1);

	float Rim_Intensity = 0;
	float Rim_Gradient = 3;
		
	//float Rim_Shadow_Area_Intensity = 0.5;
		
	#define Rim_Use_NormalMap 1
	#define Rim_Use_SpecularMap 1
	
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//See custom_subsurface.png for details

#define SUBSURFACETOON 0

	float3 Subsurface_Color = float3(1,0,0);
	
	//#define ThicknessMapTexture "thickness.png"

	float Subsurface_Toon_Intensity = 0.5;
	float Subsurface_Toon_Gradient = 2.5;
	
	float Subsurface_Rim_Intensity = 0.25;
	float Subsurface_Rim_Gradient = 0.75;
	
	//float Subsurface_Rim_Shadow_Area_Intensity = 0.5;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define PARALLAX 0

	//#define HeightMapTexture "height.png"
	
	float Parallax_Scale = 0.1;
	
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define VERTEXCOLOR 1

	float Vertex_Color_Intensity = 1;

	#define Vertex_Color_Stored_At TEXCOORD2
	//AddUV1 = TEXCOORD1
	//AddUV2 = TEXCOORD2
	//AddUV3 = TEXCOORD3
	//AddUV4 = TEXCOORD4

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define SUBMAP 0

	#define SubMapTexture "F_DIVA_STGD2NS057_NK.png"

	#define Sub_Map_Stored_At TEXCOORD1
	//AddUV1 = TEXCOORD1
	//AddUV2 = TEXCOORD2
	//AddUV3 = TEXCOORD3
	//AddUV4 = TEXCOORD4

	#define Sub_Map_Type 1
	//0: Overwrite
	//1: Shadow Map (Take dark parts to blend with the diffuse texture)
	//2: Light Map (Take bright parts to blend with the diffuse texture)
	//3: Shadow and Light Map (Take both dark and bright parts to blend with the diffuse texture)

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define IBL 1

//If you have a problem of the lighting having seams, you can use cubemap making programs like CubeMapGen to down-sample the cubemap directly, and not rely on IBL_Resolution

	#define IBLTexture "d2ns057_2_downscale.dds"
	
	float IBL_Intensity = 1;
	float IBL_Brightness = 1.05;
	float IBL_Saturation = 1;
	float IBL_Blur = 10;
	
	#define IBL_Resolution 0.1
	#define IBL_Use_NormalMap 1

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define SOFTSHADOW 2
//0: MMD's Standard Shadow
//1: Beamman's SimpleSoftShadow
//2: HariganeP's HgShadow (Must load HgShadow.x to enable soft shadow)

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define TOONSHADING 1

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define ALPHATEST 0
//Should be enabled for objects have complex texture transparency like trees... but not recommended for textures that have gradient transparency

	float Alpha_Threshold = 0.5;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#include "MES40 Sync.fxsub"