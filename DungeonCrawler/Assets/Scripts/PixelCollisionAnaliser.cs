using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.PixelPerfectCollisionDetection{
    public class PixelCollisionAnaliser : MonoBehaviour {
        
        
        public static bool CheckCollision(SpriteRenderer sprite1, SpriteRenderer sprite2)
        {
            if (Overlaps(sprite1.bounds, sprite1.bounds))
                return (PixelAnaliser(sprite1, sprite2));
            return false;
        }

        public static bool Overlaps(Bounds collider, Bounds other)
        {
            return collider.Intersects(other);
        }

        public static bool PixelAnaliser(SpriteRenderer Sprite1, SpriteRenderer Sprite2)
        {
            Vector2 m_SpriteSize1;
            Vector2 m_SpriteSize2;

            Renderer RSprite1 = Sprite1.gameObject.GetComponent<Renderer>();
            Renderer RSprite2 = Sprite2.gameObject.GetComponent<Renderer>();

            m_SpriteSize1 = GetDimensions(Sprite1);
            m_SpriteSize2 = GetDimensions(Sprite2);
            List<List<bool>> m_Mask1;
            List<List<bool>> m_Mask2;
            m_Mask1 = CreateMask(m_SpriteSize1, Sprite1);
            m_Mask2 = CreateMask(m_SpriteSize2, Sprite2);
            //quando encostar ele vai voltar true q significa colisão
        
            for(int x1 = 0; x1 < m_Mask1.Count; x1++)
            {
                for (int y1 = 0; y1 < m_Mask1[x1].Count; y1++)
                {
                    if (!m_Mask1[x1][y1]) continue;
                    for (int x2 = 0; x2 < m_Mask2.Count; x2++)
                    {
                        for (int y2 = 0; y2 < m_Mask2[x2].Count; y1++)
                        {
                            if (!m_Mask2[x2][y2]) continue;

                            Vector3 mask1PixelPosition = RSprite1.bounds.center + new Vector3(x1 * Sprite1.sprite.pixelsPerUnit, y1 * Sprite1.sprite.pixelsPerUnit, 0);

                        }
                    }
                }
            }    

            return false;
        }

        private static Vector2 GetDimensions(SpriteRenderer Sprite)
        {
            Vector2 SpriteSize = Sprite.size;
            return SpriteSize ;
        }

        private static List<List<bool>> CreateMask(Vector2 Size, SpriteRenderer Sprite)
        {
            List<List<bool>> Mask = new List<List<bool>>();
            for (int x = 0; x <= Size.x; x++)
            {
                List<bool> line = new List<bool>();
                for(int y = 0; y<= Size.y; y++)
                {
                    if(Sprite.color.a==1f)
                    {
                        line.Add(true);
                    }
                    else
                    {
                        line.Add(false);
                    }
                }
                Mask.Add(line);
            }
            return Mask;
        }
    }
}
