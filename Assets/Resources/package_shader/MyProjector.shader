Shader "Custom/MyProjector" {
 Properties { 
     _ShadowTex ("Cookie", 2D) = "" { TexGen ObjectLinear }
  }
  Subshader {
     Tags {"Queue"="Transparent-1" }//应该在透明物体之前画
     Pass {
        ZWrite Off
        AlphaTest Greater 0 //感觉Alpha值设反了
        Offset -1, -1  //应该注意像素穿插问题
        ColorMask RGB
        Blend SrcAlpha OneMinusSrcAlpha        
        SetTexture [_ShadowTex] {
            combine texture
            Matrix [_Projector]
        }
     }
  }
}
