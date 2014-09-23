float4x4 World;
float4x4 View;
float4x4 Projection;
float time;
int lightnum;
int power;
texture face;
texture pnoise;
bool renderParticles;
float3 eye;
float3 light[10];
float lightPower[10];

// TODO: add effect parameters here.

sampler TextureSampler = sampler_state{
	Texture = <face>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
};

sampler NoiseSampler = sampler_state{
	Texture = <pnoise>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
};

struct VertexShaderInput
{
    float4 pos : POSITION0;
	float4 norm : NORMAL0;
	float4 tex : TEXCOORD0;
};

struct VertexShaderOutput
{

    float4 pos : POSITION0;
	float4 tex : TEXCOORD0;
	float4 norm : TEXCOORD1;
	float4 view : TEXCOORD2;
	float4 light : TEXCOORD3;
	float4 position : TEXCOORD4;
    
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	
    float4 worldPosition = mul(input.pos, World);
    output.position = worldPosition;
    float4 viewPosition = mul(worldPosition, View);
    output.pos = mul(viewPosition, Projection);
	output.tex = input.tex;
	
    // TODO: add your vertex shader code here.
	float4 real_eye;
	float4 real_pos;
		
	real_pos = mul(input.pos,World);
	real_eye = float4(eye,1);

	output.view = normalize(real_eye-real_pos);
	output.light = float4(light[0],1);
	output.norm = normalize(mul(float4(input.norm.xyz,0),World));

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	if(!renderParticles)
	{
		float4 totalCol = float4(0,0,0,1);
		float4 ambcol = float4(0.4,0.2,0.05,1);	
		input.norm = normalize(input.norm);
		input.view = normalize(input.view);
		for (int i=0;i<1;i++)
		{
			float distance = sqrt((input.position.x - light[i].x) * (input.position.x - light[i].x) + (input.position.z -light[i].z) * (input.position.z -light[i].z));
			input.light = normalize(float4(light[i]-input.position,1));
			
			
			float3 reflect = normalize(-input.light + 2*input.norm*dot(input.light,input.norm));
			float specpart = dot(reflect,input.view);
			float diffuse = max(0,dot(input.light,input.norm));
			//if (diffuse < 0.01)
				specpart = pow(specpart,2)+diffuse;
			
			
			
			totalCol += specpart*float4(0.8,0.8,0.8,0.1)*(5/sqrt(sqrt(distance))) ;
			
			//totalCol += float4(1/distance,1/distance,1/distance,1);
		}

		totalCol = totalCol *tex2D(TextureSampler, input.tex);
		if(power== 1)
		{
			return totalCol + ambcol*tex2D(TextureSampler, input.tex);
		}
		else
		{
			return tex2D(TextureSampler, input.tex)*4;
		}
	}
	else
		return tex2D(TextureSampler, input.tex);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.
		
        VertexShader = compile vs_1_1 VertexShaderFunction();
        
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
