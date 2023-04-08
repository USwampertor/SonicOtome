Shader "Unlit/Fading"
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
    _Repeat("Repeat", Float) = 1.0
    _LineRotation("LineRotation", Float) = 0.0
    _LineThickness("LineThickness", Float) = 0.1
    _LineOffset("LineOffset", Float) = 0.0
  }
  SubShader
  {
    Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Cull back
    LOD 100

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      // make fog work
      #pragma multi_compile_fog

      #include "UnityCG.cginc"

      struct appdata
      {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f
      {
        float2 uv : TEXCOORD0;
        UNITY_FOG_COORDS(1)
        float4 vertex : SV_POSITION;
      };

      sampler2D _MainTex;
      float4 _MainTex_ST;
      float _Repeat;
      float _LineRotation;
      float _LineThickness;
      float _LineOffset;

      v2f vert(appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        UNITY_TRANSFER_FOG(o,o.vertex);
        return o;
      }

      fixed4 frag(v2f i) : SV_Target
      {
        float2 aspectRation = float2(_ScreenParams.x / _ScreenParams.y, 1.0f);
        float2 uv = frac(i.uv * _Repeat * aspectRation);

        float2 p = i.uv;

        float a1 = 1.0f;
        float b1 = tan(_LineRotation);
        float c1 = _LineOffset;

        float a2 = a1;
        float b2 = tan(_LineRotation + 3.141592f * 0.5f);
        float c2 = -a2 * p.x - b2 * p.y;

        float2 r = float2((b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1),
                          (a2 * c1 - a1 * c2) / (a1 * b2 - a2 * b1));

        float d = 1.0f - (distance(r, p) * _LineThickness);

        float circleSDF = length(uv - 0.5f);
        float color = step(d, circleSDF);

        return fixed4(color.xxx, 1.0f - color);
      }
      ENDCG
    }
  }
}
