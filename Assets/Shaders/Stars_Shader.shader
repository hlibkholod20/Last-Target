Shader "Custom/Stars Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SecondTex("Texture", 2D) = "white" {}
        _Speed("Speed", float) = 1.0
        _Speed2("Speed2", float) = 1.0
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

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
                float2 uv : TEXCOORD1;
                float2 uv2 : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Speed;
            float _Speed2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Set uvs 1
                o.uv.x = v.uv.x;

                float offset = _Time * _Speed;
                offset = offset - ((int)offset);
                o.uv.y = v.uv.y - offset;

                // set uvs 2
                o.uv2.x = v.uv.x;

                float offset2 = _Time * _Speed2;
                offset2 = offset2 - ((int)offset2);
                o.uv2.y = v.uv.y - offset2;


                return o;
            }

            sampler2D _MainTex;
            sampler2D _SecondTex;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) + tex2D(_SecondTex, i.uv2);
                return col;
            }
            ENDCG
        }
    }
}
