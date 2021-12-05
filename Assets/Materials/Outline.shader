Shader "Mobile/Outline"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _Outline ("Outline", Float) = 0.1
    }
 
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 150
 
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Front
            ZWrite On
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            uniform half4 _OutlineColor;
            uniform half _Outline;
 
            struct vertexInput
            {
                float4 vertex : POSITION;
            };
 
            struct vertexOutput
            {
                float4 pos : SV_POSITION;
            };
 
            float4 Outline(float4 vertPos, float outline)
            {
                float4x4 scaleMatrix;
                scaleMatrix[0][0] = 1.0 + outline;
                scaleMatrix[0][1] = 0.0;
                scaleMatrix[0][2] = 0.0;
                scaleMatrix[0][3] = 0.0;
                scaleMatrix[1][0] = 0.0;
                scaleMatrix[1][1] = 1.0 + outline;
                scaleMatrix[1][2] = 0.0;
                scaleMatrix[1][3] = 0.0;
                scaleMatrix[2][0] = 0.0;
                scaleMatrix[2][1] = 0.0;
                scaleMatrix[2][2] = 1.0 + outline;
                scaleMatrix[2][3] = 0.0;
                scaleMatrix[3][0] = 0.0;
                scaleMatrix[3][1] = 0.0;
                scaleMatrix[3][2] = 0.0;
                scaleMatrix[3][3] = 1.0;
 
                return mul(scaleMatrix, vertPos);
            }
 
            vertexOutput vert(vertexInput v)
            {
                vertexOutput o;
                o.pos = UnityObjectToClipPos(Outline(v.vertex, _Outline));
                return o;
            }
 
            half4 frag(vertexOutput i) : COLOR
            {
                return _OutlineColor;
            }
 
            ENDCG
        }  
 
        CGPROGRAM
        #pragma surface surf Lambert noforwardadd
 
        uniform sampler2D _MainTex;
        fixed4 _Color;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
 
    Fallback "Mobile/VertexLit"
}