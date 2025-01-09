  Shader "Test" {
            Properties {
                _Color ("Color", Color) = (1,1,1,1)
                _Outline ("Outline Color", Color) = (0,0,0,1)
                _Size ("Outline Thickness", Float) = 0.01

                _MainTex ("Albedo (RGB)", 2D) = "white" {}
                _Secondary ("Secondary map (RGB)", 2D) = "white" {}
                _Normal ("Normal map", 2D) = "bump"{}
                _NormalStr ("Normal Strength", Range(0,5)) = 1
                _Metallic ("Metallic", Range(0,1)) = 0.1
                _Glossiness ("Smoothness", Range(0,1)) = 0.1
                
                [MaterialToggle] _Saturate ("Saturate", Float) = 0
                [MaterialToggle] _InGreyScale ("Want grey scale?", Float) = 0
            }

            SubShader {
                Tags { "RenderType" = "Opaque" }
                LOD 200
  
                Pass {
                    Stencil {
                        Ref 1
                        Comp NotEqual
                    }
           
                    Cull Off
                    ZWrite Off
       
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #include "UnityCG.cginc"
                    half _Size;
                    fixed4 _Outline;

                    struct v2f {
                        float4 pos : SV_POSITION;
                    };

                    v2f vert (appdata_base v) {
                        v2f o;
                        v.vertex.xyz += v.normal * _Size;
                        o.pos = UnityObjectToClipPos (v.vertex);
                        return o;
                    }

                    half4 frag (v2f i) : SV_Target{
                        return _Outline;
                    }

                    ENDCG
                }
           }
    }