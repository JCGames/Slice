using Slice.Models.Nodes;
using Slice.Models.Nodes.BinaryOperators;
using Slice.Models.Nodes.UnaryOperators;
using Slice.Models.Nodes.ValueNodes;

namespace Slice.Parser;

public partial class Parser
{
    public static Node Analyze(Node root)
    {
        return SimplifyNode(root);
    }

    private static Node SimplifyNode(Node? node)
    {
        if (node is BlockNode blockNode)
        {
            for (var i = 0; i < blockNode.Value.Count; i++)
            {
                var simplifiedNode = SimplifyNode(blockNode.Value[i]);

                if (simplifiedNode is null)
                {
                    Diagnostics.LogError(blockNode.Meta, "Failed to simplify node.");
                    throw new InvalidOperationException();
                }
                
                blockNode.Value[i] = simplifiedNode;
            }
        }
        else if (node is ExpressionNode expressionNode)
        {
            expressionNode.Value = SimplifyNode(expressionNode.Value);
        }
        else if (node is AdditionNode additionNode)
        {
            additionNode.Value.LeftChild = SimplifyNode(additionNode.Value.LeftChild);
            additionNode.Value.RightChild = SimplifyNode(additionNode.Value.RightChild);

            {
                if (additionNode.Value is { LeftChild: IntegerNode left, RightChild: IntegerNode right })
                {
                    return new IntegerNode(left.Value + right.Value);
                }
            }
            {
                if (additionNode.Value is { LeftChild: DecimalNode left, RightChild: IntegerNode right })
                {
                    return new DecimalNode(left.Value + right.Value);
                }
            }
            {
                if (additionNode.Value is { LeftChild: IntegerNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value + right.Value);
                }
            }
            {
                if (additionNode.Value is { LeftChild: DecimalNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value + right.Value);
                }
            }
        }
        else if (node is SubtractionNode subtractionNode)
        {
            subtractionNode.Value.LeftChild = SimplifyNode(subtractionNode.Value.LeftChild);
            subtractionNode.Value.RightChild = SimplifyNode(subtractionNode.Value.RightChild);
            
            {
                if (subtractionNode.Value is { LeftChild: IntegerNode left, RightChild: IntegerNode right })
                {
                    return new IntegerNode(left.Value - right.Value);
                }
            }
            {
                if (subtractionNode.Value is { LeftChild: DecimalNode left, RightChild: IntegerNode right })
                {
                    return new DecimalNode(left.Value - right.Value);
                }
            }
            {
                if (subtractionNode.Value is { LeftChild: IntegerNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value - right.Value);
                }
            }
            {
                if (subtractionNode.Value is { LeftChild: DecimalNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value - right.Value);
                }
            }
        }
        else if (node is MultiplicationNode multiplicationNode)
        {
            multiplicationNode.Value.LeftChild = SimplifyNode(multiplicationNode.Value.LeftChild);
            multiplicationNode.Value.RightChild = SimplifyNode(multiplicationNode.Value.RightChild);
            
            {
                if (multiplicationNode.Value is { LeftChild: IntegerNode left, RightChild: IntegerNode right })
                {
                    return new IntegerNode(left.Value * right.Value);
                }
            }
            {
                if (multiplicationNode.Value is { LeftChild: DecimalNode left, RightChild: IntegerNode right })
                {
                    return new DecimalNode(left.Value * right.Value);
                }
            }
            {
                if (multiplicationNode.Value is { LeftChild: IntegerNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value * right.Value);
                }
            }
            {
                if (multiplicationNode.Value is { LeftChild: DecimalNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value * right.Value);
                }
            }
        }
        else if (node is DivisionNode divisionNode)
        {
            divisionNode.Value.LeftChild = SimplifyNode(divisionNode.Value.LeftChild);
            divisionNode.Value.RightChild = SimplifyNode(divisionNode.Value.RightChild);
            
            {
                if (divisionNode.Value is { LeftChild: IntegerNode left, RightChild: IntegerNode right })
                {
                    return new IntegerNode(left.Value / right.Value);
                }
            }
            {
                if (divisionNode.Value is { LeftChild: DecimalNode left, RightChild: IntegerNode right })
                {
                    return new DecimalNode(left.Value / right.Value);
                }
            }
            {
                if (divisionNode.Value is { LeftChild: IntegerNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value / right.Value);
                }
            }
            {
                if (divisionNode.Value is { LeftChild: DecimalNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value / right.Value);
                }
            }
        }
        else if (node is ModulusNode modulusNode)
        {
            modulusNode.Value.LeftChild = SimplifyNode(modulusNode.Value.LeftChild);
            modulusNode.Value.RightChild = SimplifyNode(modulusNode.Value.RightChild);
            
            {
                if (modulusNode.Value is { LeftChild: IntegerNode left, RightChild: IntegerNode right })
                {
                    return new IntegerNode(left.Value % right.Value);
                }
            }
            {
                if (modulusNode.Value is { LeftChild: DecimalNode left, RightChild: IntegerNode right })
                {
                    return new DecimalNode(left.Value % right.Value);
                }
            }
            {
                if (modulusNode.Value is { LeftChild: IntegerNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value % right.Value);
                }
            }
            {
                if (modulusNode.Value is { LeftChild: DecimalNode left, RightChild: DecimalNode right })
                {
                    return new DecimalNode(left.Value % right.Value);
                }
            }
        }
        else if (node is BinaryOperatorNode binaryOperatorNode)
        {
            binaryOperatorNode.Value.LeftChild = SimplifyNode(binaryOperatorNode.Value.LeftChild);
            binaryOperatorNode.Value.RightChild = SimplifyNode(binaryOperatorNode.Value.RightChild);
        }
        else if (node is UnaryOperatorNode unaryOperatorNode)
        {
            unaryOperatorNode.Value = SimplifyNode(unaryOperatorNode.Value);
        }

        return node ?? throw new InvalidOperationException("Static analysis should never result in a null simplified node.");
    }
}