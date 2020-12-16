Shader "Custom/CameraHole"{
    
    
        Properties
        {
        }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Geometry-1" }
            LOD 100
            //ZTest Always
            //ZWrite True
            //Blend Zero One
            ColorMask 0
            Pass
            {
                //ZTest Always
                //ZWrite True
                //ColorMask 0
            }
                        Pass
            {
                //ZTest Always
                //ZWrite False
                //Blend Zero One
            }
        }
    }