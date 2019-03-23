using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;
using Aiv.Fast2D.Utils.TextureHelper;
using OpenTK;
using System.IO;


namespace BulletHell4_0
{
    public static class GfxMgr
    {
        static Dictionary<string, Texture> textures;
        static Dictionary<string, Tuple<Texture, List<Animation>>> spritesheets;
        static Dictionary<string, Font> fonts;


        static GfxMgr()
        {
            textures = new Dictionary<string, Texture>();
            spritesheets = new Dictionary<string, Tuple<Texture, List<Animation>>>();
            fonts = new Dictionary<string, Font>();
        }


        static public void LoadAll()
        {
            Load();
            LoadFont();
        }

        static XmlNode Root(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            XmlNode root = xmlDoc.DocumentElement;
            return root;
        }
        
        static XmlNode NextNode(XmlNode node)
        {
            return node = node.NextSibling;
        }

        #region Animation/Spritesheet
        static Animation LoadAnimation(XmlNode animationNode, int width, int height)
        {
            XmlNode currentNode = animationNode.FirstChild;
            string name = currentNode.InnerText;
            currentNode = currentNode.NextSibling;
            bool loop = bool.Parse(currentNode.InnerText);
            currentNode = currentNode.NextSibling;
            int fps = int.Parse(currentNode.InnerText);
            currentNode = currentNode.NextSibling;
            int cols = int.Parse(currentNode.InnerText);
            currentNode = currentNode.NextSibling;
            int rows = int.Parse(currentNode.InnerText);
            currentNode = currentNode.NextSibling;
            int startX = int.Parse(currentNode.InnerText);
            currentNode = currentNode.NextSibling;
            int startY = int.Parse(currentNode.InnerText);

            return new Animation(width, height, name, rows, cols, fps, loop, startX, startY);
        }

        static public void LoadSpriteSheet(XmlNode spritesheetNode)
        {
            //name
            XmlNode nameNode = spritesheetNode.FirstChild;
            string name = nameNode.InnerText;

            //texture
            XmlNode filenameNode = nameNode.NextSibling;
            Texture texture = new Texture(filenameNode.InnerText);
            Addtexture(name, filenameNode.InnerText);

            //animation -> frameW-H, rows, cols, fps, loop
            XmlNode frameNode = filenameNode.NextSibling;

            if (frameNode.HasChildNodes)
            {
                List<Animation> animation = new List<Animation>();
                int framewidth = int.Parse(frameNode.FirstChild.InnerText);
                int frameheight = int.Parse(frameNode.LastChild.InnerText);
                XmlNode animationsNode = frameNode.NextSibling;
                foreach (XmlNode item in animationsNode)
                {
                    if (!item.HasChildNodes)
                        animation.Add(new Animation(framewidth, frameheight));
                    else
                        animation.Add(LoadAnimation(item, framewidth, frameheight));
                }
                AddSpritesheet(name, texture, animation);
            }
            else
                AddSpritesheet(name, texture);
        }

        static public void AddSpritesheet(string name, Texture texture, List<Animation> animation)
        {
            spritesheets.Add(name, new Tuple<Texture, List<Animation>>(texture, animation));
        }

        static public void AddSpritesheet(string name, Texture t)
        {
            List<Animation> a = new List<Animation>();
            a.Add(new Animation(t.Width, t.Height));
            AddSpritesheet(name, t, a);
        }

        static public Tuple<Texture, List<Animation>> GetSpritesheet(string name)
        {
            if (spritesheets.ContainsKey(name))
            {
                List<Animation> newAnimations = new List<Animation>();
                //Lambda -> ConvertAll(a 
                for (int i = 0; i < spritesheets[name].Item2.Count; i++)
                {
                    newAnimations.Add((Animation)spritesheets[name].Item2[i].Clone());
                }

                Tuple<Texture, List<Animation>> clonedSS = new Tuple<Texture, List<Animation>>(spritesheets[name].Item1, newAnimations);

                return clonedSS;
            }
            return null;
        }

        static public void Load(string fileName = "Assets/XML/SpritesheetConfig.xml")
        {
            XmlNode root = Root(fileName);

            foreach (XmlNode spritesheetNode in root.ChildNodes)
            {
                LoadSpriteSheet(spritesheetNode);
            }
        }
        #endregion

        #region Font
        static public void LoadFont(string fileName = "Assets/XML/FontsConfig.xml")
        {
            XmlNode root = Root(fileName);
            XmlNode fontNode = root.FirstChild;

            while (fontNode != null)
            {                
                XmlNode currentNode = fontNode.FirstChild;

                string name = currentNode.InnerText;
                currentNode = NextNode(currentNode);
                string filename = currentNode.InnerText;
                currentNode = NextNode(currentNode);
                int firstVal = int.Parse(currentNode.InnerText);
                currentNode = NextNode(currentNode);
                int cols = int.Parse(currentNode.InnerText);
                currentNode = NextNode(currentNode);
                int rows = int.Parse(currentNode.InnerText);
                currentNode = NextNode(currentNode);
                float scale = float.Parse(currentNode.InnerText);

                Texture texture = new Texture(filename);
                AddSpritesheet(name, texture);

                float charWidth = texture.Width / cols;
                float charHeight = texture.Height / rows;

                Font f = new Font(name, filename, cols, firstVal, charWidth, charHeight, scale);

                fonts.Add(name, f);
                
                fontNode = NextNode(fontNode);
            }
        }

        static public void AddFont(string fontName, Font f)
        {
            fonts.Add(fontName, f);
        }

        static public Font GetFont(string fileName)
        {
            Font f = null;
            if (fonts.ContainsKey(fileName))
            {
                f = fonts[fileName];
            }
            return f;
        }
        #endregion

        #region Texture
        static public void AddTexture(string name, string filePath)
        {
            textures.Add(name, new Texture(filePath));
        }
        static public Texture Addtexture(string name, string filepath)
        {
            string ext = Path.GetExtension(filepath);
            string texFilepath = filepath.Replace(ext, ".tex");
            if (!File.Exists(texFilepath))
            {
                string dirName = Path.GetDirectoryName(filepath);
                TextureHelper.GenerateDecompressedTextureFromFile(filepath, dirName);
            }
            Texture t = TextureHelper.LoadDecompressedTexture(texFilepath);
            textures.Add(name, t);
            return t;
        }

        static public Texture GetTexture(string name)
        {
            if (textures.ContainsKey(name))
            {
                return textures[name];
            }
            return null;
        }
        #endregion
    }
}
