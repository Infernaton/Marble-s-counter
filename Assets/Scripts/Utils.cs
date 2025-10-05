using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils 
{
    static public class Random
    {
        private static System.Random rng = new System.Random();
        public static T[] Shuffle<T>(T[] list)
        {
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }

    static public class Compare
    {
        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }
    }

    public class Anim
    {
        private static IEnumerator AnimationOnCurve(float time, Action<float> animation, AnimationCurve curve) {
            float currentTime = 0f;

            while (currentTime < time)
            {
                animation(curve.Evaluate(currentTime / time));
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        private static IEnumerator AnimationOnSinus(float time, Action<float> animation, float magnitude, float speed)
        {
            float currentTime = 0f;
            //Because we want the full animation on the time provided
            // and its take 2pi for the sinus function to make it normally
            float sinusCoeff = (2 * Mathf.PI/ time);

            while (currentTime < time)
            {
                animation(Mathf.Sin((currentTime / time) * speed * sinusCoeff) * magnitude);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        #region FadeIn/FadeOut
        #region TMP_Text
        public static IEnumerator FadeIn(float t, TMP_Text txt)
        {
            txt.enabled = true;
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
            while (txt.color.a < 1.0f)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }

        public static IEnumerator FadeOut(float t, TMP_Text txt)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
            while (txt.color.a > 0.0f)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime / t));
                yield return null;
            }
            txt.enabled = false;
        }
        #endregion

        #region CanvaGroup
        public static IEnumerator FadeIn(float t, CanvasGroup c)
        {
            c.gameObject.SetActive(true);
            c.alpha = 0f;
            while (c.alpha < 1.0f)
            {
                c.alpha += Time.deltaTime / t;
                yield return null;
            }
        }

        public static IEnumerator FadeOut(float t, CanvasGroup c)
        {
            c.alpha = 1f;
            while (c.alpha > 0.0f)
            {
                c.alpha -= Time.deltaTime / t;
                yield return null;
            }
            c.gameObject.SetActive(false);
        }
        #endregion

        #region RawImage
        public static IEnumerator FadeIn(float t, RawImage i)
        {
            i.gameObject.SetActive(true);
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }

        public static IEnumerator FadeOut(float t, RawImage i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
            i.gameObject.SetActive(false);
        }
        #endregion

        #region Image
        public static IEnumerator FadeIn(float t, Image i)
        {
            i.gameObject.SetActive(true);
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }

        public static IEnumerator FadeOut(float t, Image i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
            i.gameObject.SetActive(false);
        }
        #endregion
        #endregion

        public static IEnumerator Blink(GameObject obj, float time)
        {
            Renderer[] objectRenderers = obj.GetComponentsInChildren<Renderer>();
            bool switchAnim = true;
            float endTime = Time.time + time;
            while (endTime > Time.time)
            {
                switchAnim = !switchAnim;
                BlinkAnim(objectRenderers, switchAnim);
                yield return new WaitForSeconds(0.05f);
            }
            //To make sure the gameobject stay visible at the end of the animation
            BlinkAnim(objectRenderers, true);
        }

        private static void BlinkAnim(Renderer[] objectRenderers, bool hasToRender)
        {
            foreach (Renderer r in objectRenderers)
                r.enabled = hasToRender;
        }

        #region PopIn/PopOut
        public static IEnumerator PopIn(float t, CanvasGroup i)
        {
            i.gameObject.SetActive(true);
            yield return Pop(t, i.gameObject, 1f);
        }
        public static IEnumerator PopOut(float t, CanvasGroup i)
        {
            yield return Pop(t, i.gameObject, 0f);
            i.gameObject.SetActive(false);
        }
        private static IEnumerator Pop(float t, GameObject o, float targetScale)
        {
            Vector3 from = o.transform.localScale;
            Vector3 to = Vector3.one * targetScale;
            yield return AnimationOnCurve(t, t => o.transform.localScale = Vector2.Lerp(from, to, t), AnimationCurve.Linear(0, 0, 1, 1));
        }
        #endregion

        #region Giggle
        public static IEnumerator Giggle(float t, Transform i, float magnitude, float speed)
        {
            Vector2 basePos = i.localPosition;
            yield return AnimationOnSinus(t, t => i.localPosition = basePos + Vector2.one * t, magnitude, speed);
            i.localPosition = basePos;
        }
        #endregion

        #region MovesTo
        public static IEnumerator MoveUI(RectTransform o, Vector2 to, float time, AnimationCurve curve)
        {
            Vector2 from = o.anchoredPosition;
            yield return AnimationOnCurve(time, t => o.anchoredPosition = Vector2.Lerp(from, to, t), curve);
        }

        public static IEnumerator MoveObject(Transform o, Vector3 to, float time, AnimationCurve curve)
        {
            Vector3 from = o.position;
            yield return AnimationOnCurve(time, t => o.position = Vector3.Lerp(from, to, t), curve);
        }
        #endregion

        public static IEnumerator ChangeColor(float t, TMP_Text text, Color goalColor, AnimationCurve animation)
        {
            Color current = text.color;
            yield return AnimationOnCurve(t, t => text.color = Color.Lerp(current, goalColor, t), animation);
        }
    }
}
