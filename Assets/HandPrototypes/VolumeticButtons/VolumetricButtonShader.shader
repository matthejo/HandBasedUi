Shader "Unlit/VolumetricButtonShader"
{
    Properties
    {
		_Test("Test", Range(0, 1)) = .5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 objSpace : TEXCOORD1;
            };

			float _Test;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.objSpace = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float pressVal = lerp(.5, -.5, _Test);

				float ret = i.objSpace.z;
				clip(ret + pressVal);
				ret = ret * .5 + .5;
				return ret;
            }
            ENDCG
        }
    }
}
