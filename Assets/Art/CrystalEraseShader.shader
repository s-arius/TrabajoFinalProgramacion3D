Shader "Custom/CrystalEraseShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _EraseMask ("Erase Mask", 2D) = "white" {} 
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _EraseMask;
            float4 _BaseColor;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float mask = tex2D(_EraseMask, i.uv).r;  
                return float4(_BaseColor.rgb, mask);     // alpha = máscara
            }
            ENDCG
        }
    }
}
