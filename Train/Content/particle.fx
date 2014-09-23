float4x4 World;
float4x4 View;
float4x4 Projection;

Texture pic;

sampler textureSampler = sampler_state{
Texture = <pic>; 
magfilter = LINEAR;
minfilter = LINEAR;
mipfilter = LINEAR;
};

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float1 Size : PSIZE0;
    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float1 Size : PSIZE;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position =/* mul(viewPosition, Projection) + */float4(input.Position.x,input.Position.y,1,1);

    // TODO: add your vertex shader code here.
	output.Size = input.Size;
	output.Color = float4(input.Color.xyz, min(1,0.5)); //float4(0.8f, 0.7f, 0.3f, fx_fade);
	output.UV = float2(1.0f, 1.0f);
    return output;
}
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float2 UV = input.UV.xy;	
	return (tex2D(textureSampler, UV)*input.Color);
//    return float4(1, 0, 0, 1);
}
 
technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

		//PointSpriteEnable = true;
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = One;

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
