Shader "Unlit/GrabbableOutlineShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
		ZWrite Off
		Blend One One

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float ret = 1 - (abs(i.uv.y - .5) * 2);
			ret = pow(ret, .5);
				ret *= i.vertex.z * 20;
				return ret;
            }
            ENDCG
        }
    }
}
