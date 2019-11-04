Shader "Unlit/FakeVideoShader"
{
    Properties
    {
		_Fade("Fade", Float) = 1
        _MainTex ("Texture", 2D) = "white" {}
		_Corners("Corners", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
			float _Corners;
			float _Fade;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			float GetCardAlpha(float2 uv)
			{
				float2 dists = abs(uv - .5) * 2;
				dists = 1 - dists;
				dists.x *= 1.7778;
				dists.x = max(0, dists.x);
				dists = 1 - saturate(dists);
				dists = pow(dists, _Corners);
				float ret = dists.x + dists.y;
				ret = saturate(ret);
				ret = pow(ret, 2);
				ret = 1 - ret;
				return ret;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float baseAlpha = GetCardAlpha(i.uv);
                fixed3 col = tex2D(_MainTex, i.uv).xyz;
				baseAlpha *= _Fade;
                return float4(col, baseAlpha);
            }
            ENDCG
        }
    }
}
