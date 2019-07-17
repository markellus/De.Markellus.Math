using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.Arithmetic;
using System;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    /// <summary>
    /// Node für den Gleichheits-Operator.
    /// </summary>
    public class EqualityNode : BinaryTermNode
    {
        /// <summary>
        /// true, wenn der Operator invertiert werden soll (Ungleichheit), ansonsten false.
        /// </summary>
        public bool Inverted { get; }

        /// <summary>
        /// Erstellt eine neue Instanz und initialisiert alle Kinder mit null.
        /// </summary>
        public EqualityNode() : base()
        {
            Inverted = false;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="left">Das linke Kind des Nodes</param>
        /// <param name="right">Das rechte Kind des Nodes</param>
        public EqualityNode(TermNode left, TermNode right, bool bInverted) : base(left, right)
        {
            Inverted = bInverted;
        }

        /// <summary>
        /// Löst den Node in eine reelle Zahl auf.
        /// </summary>
        /// <returns>Eine reelle Zahl, die das Ergebnis des (Teil)-Termes ist der durch diesen
        /// Node repräsentiert wird.</returns>
        public override Real Resolve()
        {
            return (Inverted ? LeftChild.Resolve() != RightChild.Resolve() : LeftChild.Resolve() == RightChild.Resolve()) ? "1" : "0";
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is EqualityNode node && Equals(node);
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(OperatorAddNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Erstellt eine exakte Kopie des Nodes.
        /// </summary>
        /// <returns>Eine exakte Kopie des Nodes.</returns>
        public override TermNode CreateCopy()
        {
            return new EqualityNode(LeftChild.CreateCopy(), RightChild.CreateCopy(), Inverted);
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EqualityNode left, EqualityNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EqualityNode left, EqualityNode right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return LeftChild + "=" + RightChild;
        }
    }
}
