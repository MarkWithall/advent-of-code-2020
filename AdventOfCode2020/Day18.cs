﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 18: Operation Order")]
    public sealed class Day18
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(7147789965219, Day18Input.Sum(EvaluateExpression));
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Part1Sample(string expression, long expectedResult)
        {
            Assert.AreEqual(expectedResult, EvaluateExpression(expression));
        }

        [Test]
        public void Part2()
        {
            Assert.AreEqual(136824720421264, Day18Input.Sum(EvaluateExpression2));
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 231)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 46)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Part2Sample(string expression, long expectedResult)
        {
            Assert.AreEqual(expectedResult, EvaluateExpression2(expression));
        }

        private static long EvaluateExpression(string expression)
        {
            var tokens = Lex(expression).ToArray();
            return Parse(new Tokens(tokens)).Evaluate();
        }

        private static INode Parse(Tokens tokens)
        {
            return ParseBinary();

            INode ParseBinary()
            {
                var lhs = ParseLeaf();

                while (true)
                {
                    Func<long, long, long>? op = tokens.CurrentToken switch
                    {
                        PlusToken => (a, b) => a + b,
                        MultiplyToken => (a, b) => a * b,
                        _ => null
                    };

                    if (op is null)
                    {
                        return lhs;
                    }

                    tokens.MoveNext();
                    lhs = new BinaryOpNode(lhs, ParseLeaf(), op);
                }
            }

            INode ParseLeaf()
            {
                if (tokens.CurrentToken is NumberToken number)
                {
                    tokens.MoveNext();
                    return new NumberNode(number.Value);
                }

                if (tokens.CurrentToken is OpenParenthesisToken)
                {
                    tokens.MoveNext();
                    var node = ParseBinary();
                    if (tokens.CurrentToken is not CloseParenthesisToken) throw new InvalidOperationException("Missing close parenthesis");
                    tokens.MoveNext();
                    return node;
                }

                throw new InvalidOperationException("Not a leaf");
            }
        }

        private static long EvaluateExpression2(string expression)
        {
            var tokens = Lex(expression).ToArray();
            return Parse2(new Tokens(tokens)).Evaluate();
        }

        private static INode Parse2(Tokens tokens)
        {
            return ParseMultiply();

            INode ParseMultiply()
            {
                var lhs = ParseAdd();

                while (true)
                {
                    Func<long, long, long>? op = tokens.CurrentToken switch
                    {
                        MultiplyToken => (a, b) => a * b,
                        _ => null
                    };

                    if (op is null)
                    {
                        return lhs;
                    }

                    tokens.MoveNext();
                    lhs = new BinaryOpNode(lhs, ParseAdd(), op);
                }
            }

            INode ParseAdd()
            {
                var lhs = ParseLeaf();

                while (true)
                {
                    Func<long, long, long>? op = tokens.CurrentToken switch
                    {
                        PlusToken => (a, b) => a + b,
                        _ => null
                    };

                    if (op is null)
                    {
                        return lhs;
                    }

                    tokens.MoveNext();
                    lhs = new BinaryOpNode(lhs, ParseLeaf(), op);
                }
            }

            INode ParseLeaf()
            {
                if (tokens.CurrentToken is NumberToken number)
                {
                    tokens.MoveNext();
                    return new NumberNode(number.Value);
                }

                if (tokens.CurrentToken is OpenParenthesisToken)
                {
                    tokens.MoveNext();
                    var node = ParseMultiply();
                    if (tokens.CurrentToken is not CloseParenthesisToken) throw new InvalidOperationException("Missing close parenthesis");
                    tokens.MoveNext();
                    return node;
                }

                throw new InvalidOperationException("Not a leaf");
            }
        }

        private sealed class Tokens
        {
            private LinkedListNode<IToken>? _current;

            public Tokens(IEnumerable<IToken> tokens)
            {
                _current = new LinkedList<IToken>(tokens).First;
            }

            public IToken? CurrentToken => _current?.Value;

            public void MoveNext() => _current = _current?.Next;
        }

        private static IEnumerable<IToken> Lex(string expression)
        {
            var i = 0;
            while (i < expression.Length)
            {
                var number = new string(expression.Skip(i).TakeWhile(char.IsDigit).ToArray());
                if (number.Length > 0)
                {
                    yield return new NumberToken(long.Parse(number));
                    i += number.Length;
                }
                else
                {
                    switch (expression[i])
                    {
                        case ' ':
                            break;
                        case '(':
                            yield return new OpenParenthesisToken();
                            break;
                        case ')':
                            yield return new CloseParenthesisToken();
                            break;
                        case '+':
                            yield return new PlusToken();
                            break;
                        case '*':
                            yield return new MultiplyToken();
                            break;
                    }

                    i++;
                }
            }
        }

        private interface INode
        {
            long Evaluate();
        }

        private sealed record NumberNode(long Number) : INode
        {
            public long Evaluate() => Number;
        }

        private sealed record BinaryOpNode(INode Lhs, INode Rhs, Func<long, long, long> Op) : INode
        {
            public long Evaluate() => Op(Lhs.Evaluate(), Rhs.Evaluate());
        }

        private interface IToken
        {
        }

        private sealed record NumberToken(long Value) : IToken;

        private sealed record OpenParenthesisToken : IToken;

        private sealed record CloseParenthesisToken : IToken;

        private sealed record PlusToken : IToken;

        private sealed record MultiplyToken : IToken;

        private static readonly string[] Day18Input =
        {
            "6 * 8 * (2 * 9) + 2 * 8 * 4",
            "(2 + 2 * 4 * (8 * 9 * 2 + 6) * 4) * 2",
            "2 * (7 + 3 * 5 * 7 * 6) + 9 * 4",
            "((8 * 4 * 7 + 9 + 8) * (6 * 4) + 7 * 9 * 8 + 3) * (9 + 6 + 8 + 2 + (6 * 4 * 6) * (9 + 7 * 6 * 5 + 5 * 3)) * 8 * 8",
            "3 + 8 * (4 * 8 * (9 + 3 + 9 * 5 + 6 * 7)) * 6 + 6 * (4 + 6 * 9 + 9 * (4 * 8) + 2)",
            "7 * 8 * 3 + (8 + 9 + 3 * 3 * 4 * 5) * 5",
            "(2 * 2 + 2 * 8 + 7 * 5) + 2 + 5 + 2",
            "2 + ((3 * 7 + 8 * 9) * 6 * 5 + (6 * 3 * 3 + 6 + 6 + 4) * (7 * 3 + 7 * 7 * 9)) * (2 * 2) + 6",
            "9 * 5 + 5 + 9 + 3",
            "(9 * 3 * (9 + 4 * 4 + 6) + 7) + ((2 * 9 + 8 + 5) * 3 * 3) + 8",
            "(6 * 8) * 5 + (7 + 2 * 9) * 9 * 4",
            "2 * 3 + ((6 + 8 + 8) + 5 + 8)",
            "4 * (6 * 4 * 5 + (9 * 3 + 4 + 2) * 2)",
            "(7 * 4 + (6 * 8 + 9 + 4 * 9 * 5) + 3 * 6 + (2 + 8 * 9 * 4 * 6)) + 7 + 9 * (6 + (5 + 8 + 9 * 6 + 5) * 9 * 7 + 2) + 4",
            "((3 + 2) + 7 + (6 + 3 * 7) + 7 * (5 * 6 * 9 * 4) * 5) + (2 * (9 + 8 * 4 * 7) * 7 * 6) * 7",
            "((6 + 6) * 2) * 9 + 3 + 5",
            "((2 * 4 + 8) + 9 * 8 * 7 * 2) + (6 * 5 + 4 + (4 + 3 + 5)) + 6 * 6 * 5 * ((9 * 4 * 7) * 6 + 8 * (6 * 2))",
            "7 + 8 + 6",
            "6 + (4 * 2 * 6 + (7 + 7 + 3 * 3 + 9) * 6) * 5 + (7 * 7) + 2",
            "8 + 2 + (3 + 9 * 4 * 4 + 3 + 5) + 3 + (6 + 4 + 3 * 9 * 2) + 7",
            "(5 * 5) + (6 + 4 + 8 * 3 * 6 + 7) * 5",
            "(3 * (4 + 7 + 9 * 8 + 7) * 8) + 3",
            "3 * (3 + 6)",
            "8 * 4 * 8 + (2 * 3 + 8 * (4 + 2) + 2 * 7)",
            "8 + (4 * 6 * 6 + 7 * (7 * 5 + 2))",
            "(4 + 2 * 9 * 7 * (2 * 8)) + 8 * 8 * 6 + 4",
            "9 + 3 * 9 + 7",
            "(7 + (5 * 5 * 2 + 4 + 7 * 2) * 3 * (5 + 4 * 7 + 2)) + 5 * (7 + 9) + 2 + 9 + 3",
            "(4 * (7 + 3 * 6 * 3 * 5)) + 5 * ((5 * 4 * 2) + 9 * 3 + 6) + 7 + (5 * 6 * 6 * 3 * 7)",
            "3 * (8 * (6 * 8 + 8 + 5))",
            "6 * (7 + (2 + 2 * 5 * 3) * 3) + (2 * 9 * 4 * 4)",
            "2 * 7 + ((4 * 5 + 3) + 2) + (3 + 5 * 2) + 8",
            "((2 + 6 * 4 * 3) * 6 * 5 * 6 + 8 * 5) + 5 * 8",
            "4 * (6 + 9) + 6 + 8 * 5",
            "(3 * (2 + 8) + (6 * 3 * 8) + (5 * 3 * 6 + 9 + 8 + 8)) + 7",
            "5 * 4 + ((6 + 3) + 6) + 3 * (2 + (4 + 9 * 9 * 5) * (4 * 3) + 7 + 3) + 3",
            "(3 + 5) * 9 * 4",
            "6 * (4 + (2 * 7 * 8 * 5 * 6) + 6 * 7 * 2 + 6) + 7",
            "4 * 7 * (6 + 2 * 4 * 6) + 5 + 6 * ((5 * 9 + 3) + 7)",
            "2 * 2 + 5 * 8 + ((9 + 2 + 4) * 4 + 2 * 6 + 5) + 3",
            "(5 * 4 + 2) * 2 * (7 * (7 * 6 + 9 * 8) * (5 * 7 * 9))",
            "(2 * 2) + 8 * 9 * 6 + (4 + (8 + 8 * 4 + 5 * 3) + (9 + 5) * 2 + (4 + 8 * 4 * 9 + 8)) + 2",
            "4 + (5 + 3 + 4 + (7 * 6 + 5 * 4 + 5) * 9) * 3 + 8 + 3",
            "9 * 3 * 3 + (6 + (6 + 9 + 7 * 5 + 9) * 9) * 3 * (6 * 5 * 6 + 2)",
            "(7 * 3 + 5 + 8) * 3 * (7 * 6 + 4 + (6 * 5 + 4 * 2 + 5)) * 4",
            "3 + 5 + (7 + 7 * 8 + 7 + (5 * 9 * 9 + 4 * 5) + 6) + 2 * 9 + 2",
            "3 * (6 + 6 + 5 * 5 * 5) * 4 * (3 + 5 + 5 * 5) * 6 + (8 + 6 + (5 * 2 + 9) + 6 * (5 * 4 + 7))",
            "3 + 9 * (5 + 7 * 8 * 5 * 8 * 2) * 9",
            "3 + 5 + 3 * 2 * (5 * 5) + 4",
            "6 + (4 * (9 * 9 * 4 * 4) * (2 * 6 * 9 + 8) + 5 + 2 * (6 + 8 * 6 + 3 * 9)) + 9",
            "(6 + (6 * 3 + 9 + 2 * 5) * 6 + 4 * 5 * 4) * 5 * 5 * 9 + 6 + 5",
            "(6 + 2 + 8 * (7 + 8 * 5 * 8 + 2) + (5 + 5 + 3 * 4 * 6) + 8) * 8 * 9 * 8",
            "8 + ((7 + 5 * 5 + 8 * 4) * 6 + 3 * 7 + 4 * 6) * 8",
            "6 * 7 * 7 + ((6 + 6 + 8) + 3 + 3) * 4 * 4",
            "9 * 3 * ((3 * 9 * 2 + 2 * 2) * 8) + 4",
            "(9 + 9 + 3) * 8 + 5 * 9 + ((3 + 4 + 6 + 7 + 2) * 7 + 8 * 3 + 3 + (2 * 8 + 5 + 3 + 3 + 8)) * (2 * 7)",
            "(2 + 8 + (9 * 6 * 9 + 3) + 2 * 4) + 4 * 7 * 8 + 4",
            "8 + 3 + (8 * 3 + 7) * 6",
            "(3 * 5 * 2 * 8 * (5 + 4 * 6 + 8)) * 6 * (3 + 9 + 4 + 4) * (7 + 6 * 6)",
            "9 + (4 + 5 + (6 * 2 + 4)) * (6 + 5 + 9)",
            "5 * 3 * (9 * (8 * 4 * 5 + 8) + 4 + 5 + (6 + 5 + 5 + 3 + 8 + 3))",
            "6 * (2 * 5 * 9 * 2) + (8 * 6 + 7)",
            "7 + 6 + 9 + (8 + 5 + 8 * 2 * 9) * 6",
            "4 + (3 * (7 + 9 + 5 + 2 + 7)) * (8 * 3) * 9 + 5 + (5 + 6 * 7 + 9)",
            "5 * 5 * 4 + ((4 * 5 * 8 + 4 + 5 + 8) * 7 + (6 + 7) + 6 * 4 + 2) + (3 * 4 * 2 + 3 + 2 + 6) + 2",
            "(4 * 2 + (6 * 9 + 7) + 3) * 5",
            "5 * ((8 * 8) + 6 + 8 * 6) * ((6 + 5 * 5 + 6 + 3 * 8) * 9 + 3 + (7 + 7 + 5 * 2))",
            "((5 * 8) + 7 + (4 + 3 + 6 * 3) + (9 * 9 * 9 + 2 * 6) + 5) + (7 + 5 + 7 * (5 + 5 + 7) + 9 * 2) * 3 + 9",
            "7 * 9 + 3",
            "2 * (7 + 8 * 3) + 3 + 4 * (7 * (5 * 2 + 8 + 5) + 9 + 2)",
            "(9 + 5 * (9 + 5 * 7 + 6)) + 9 + 7 + 4 * (6 + 9 + 9 + 3)",
            "7 + (9 * 8 + (9 + 2 + 8 + 3 + 4) + 4 * 4 * 9) + 7 + (7 * 8 + 3 + 9)",
            "5 * ((8 * 8 * 4 + 4) * 4 * (6 * 9 + 8 + 7 * 2 + 7) * 6 + 2) + 7",
            "6 * 7 + 3 * (5 + 7 * 3 * 7) * (8 + (5 * 8 + 5 * 3))",
            "7 + 8 * (7 * 2 * 3 + 5 + 3 * (9 + 8 * 2 * 3 * 8)) + 8 + 3 + 4",
            "2 * (4 * (3 * 4 + 5 + 4 * 4) * 4 + 2 * 2 * 3)",
            "(7 * 2 + 3 + 3 + (8 * 3 * 8 * 6 * 9 * 8)) + 7 * 9 * 7 + 9",
            "6 + 3 * (9 + 2 + 9) + (5 + 8 * 8)",
            "3 + 8 + (7 * 9 + 4 + 5) + 6",
            "(4 + 3 * (5 * 7 * 6 + 5 * 8)) * 8 + 3 + 5 + 3",
            "8 + ((9 * 4 + 5) + 4) * 2 * 2 * 3 + 4",
            "(5 * 8) + 3",
            "4 + (4 + (2 + 3 + 2 * 8 * 4 * 4) * 7 * 5 * 5 + 3) * 9",
            "(9 + 6 + 3 * 7 + 6 + 9) * 9 + (4 + 8 * (6 + 3 * 6 * 5) + 3 + 5 + (8 * 8 * 8 * 6)) + 5",
            "4 * (9 + 8 * 4 * (6 + 8 * 4 + 7) + 5) * 4 * (4 * 8 + 7) + 4 + 3",
            "5 * 5 + 3 * 3 * (2 * 5 * (2 + 8 + 7) + (2 + 8) + 6)",
            "(9 + 6 * 8 + (4 * 7 + 8 + 5 + 2) + 3 * 2) + 3 * (2 * (6 + 7 + 3 + 4 + 6) + 8 + 5 * 2) * (8 * (4 * 7 + 5 * 7) + 4) + 5",
            "9 + (2 * (5 * 7) * (5 * 7 * 7) * 7 * 4) + 7 * (5 * 9 + (9 * 7 * 7 * 2))",
            "5 * 8 + 4 + (4 * 4) + 7 * 7",
            "6 + (3 + (2 + 6) + 3) + 2",
            "(3 + 9 * (7 + 4 * 8 * 8) + (4 + 9) * 9 * 2) + 9",
            "6 * 4 * ((4 + 3 * 5 * 4) * 3 + (5 + 7 + 6 + 8 * 6) + (2 + 3 + 2 + 3 * 3) * (6 * 8 * 2)) * (3 * (9 * 8 * 4 + 4 * 3 + 3) + (4 + 8 + 9))",
            "3 * 2 * (8 * (8 + 6) * 6) + (7 + 2 + 6 * 3 + 4 * 4) + (7 + 4 + 2 + 8 * 9) + 2",
            "(2 + 7 + (6 + 3) + 4 + 3) + (3 * 8 + 5 + 7 * 6) * 2 * 6 * 8 + (4 + (9 + 5 + 4 + 5 + 2 * 5) + 3)",
            "(9 * 2 * 5) + 8 * 7 + (8 * 8 * (5 * 8 * 3 * 6) + 7)",
            "7 * 3 * 8",
            "6 + (6 * 7) + 9 + 9 + (6 * 5 + 3 * (8 * 2 * 4) + 9)",
            "6 + 3 * 6 + ((9 + 4) * (6 + 7 * 2)) * 3",
            "6 + 3 + (4 + 5 * 4 + 8) + 5 + 9 + 5",
            "(5 * 2 + 3 * 2 + 3) * 6 * 4 * 7 + 2 * 2",
            "4 * 7 * ((5 * 9 + 8 * 6) + 6 + 3 * (4 * 9) * 4)",
            "(8 * 4) + 5 + 5 + (6 + 4 * (5 * 7 * 5 + 4 * 9 + 9) + (2 * 3 * 7 + 8 + 2) + (7 * 4 * 8)) + 2 * 3",
            "7 * 6 + 7 + (2 * 6 * 2 * 5 * 8)",
            "7 * 2 + ((7 + 9 * 3) + 4 + 2) * 6 * 7 * ((8 + 8) + 7)",
            "4 + (9 * 2 + 2 * (9 * 3 + 3 + 3 * 7) + 8 + 7) + 5",
            "8 * 8 * 9 * 8 * (2 * 6 + (9 + 4 + 5) * (9 * 4 + 9 * 7 + 2) + 2) * (2 * 6 + 3 * 6 + 3 + (4 + 5 * 6 * 3))",
            "4 * 9 * 3 + 2 + (8 + 5 * 8 * 5 * (4 * 8 * 8 * 8) + (5 + 6)) * 2",
            "9 * 2 * 8 * 5 * 2 + 7",
            "7 + 8 * (9 * 9 * 3 + (5 * 6 * 9 * 4 * 8) + 3 * 6) + ((7 * 2 * 5 * 3) * 3 + (2 * 5 + 9 + 8 * 4) * 6 + 9 + 5) + 3",
            "3 + (3 + 8 * 3 + 3 * 6 + (2 + 8)) * 2 * (9 * (3 + 2) + (3 + 9 + 7) * 7 + (9 + 3 * 7 + 2)) * ((8 * 2) * 3 * 8 + 3)",
            "2 * (7 * 7 * 8 * 6 + 6) * 6 + 2 * 9 * (3 * 8 * 9)",
            "(6 * 3 * 5 + (4 * 3 * 8 * 9 * 6 + 4) * 6) * 8 + 6",
            "6 + 6 * 5 * 7 + 3",
            "2 * 2 + 8 + 8 + (8 + 9 + (5 + 7 + 4 * 6) + 8 + 2) * 7",
            "6 * (7 + (2 * 2 * 8 * 8 * 9 + 6) + 3 * 3) * 3 * 5 + 4 + 9",
            "4 + (7 * 6 * 4 + (9 + 8 * 8 + 6) * (8 + 3 * 7 * 3 + 6 * 4) + (9 + 6 * 2)) * (9 + 8 * 6) * 8 * 4",
            "(3 * (9 + 6 * 9 * 2) + 9 + 5 + 7 + 9) * 3 * 2 * 6 * 4 * 6",
            "(8 * 9 + 9 + 8) * 6 * (6 + 2 * 4 + (9 + 4 + 2 + 9) * (9 + 7)) + 3 * 5 * 3",
            "9 * 6",
            "2 + 3 + ((4 + 3 + 5 * 2 * 3) * 5 * 5 + 5 + 8) * 7 + (5 * 2)",
            "2 * (5 + 6 * 7) + 6 * 5 + 5 * (7 + 6)",
            "7 + 9 + 8 + 9 * ((4 + 5 * 5 + 3) * 5 * 9 + (6 + 7 + 4) + 5) * (7 + 2 + 5)",
            "7 * ((2 + 6) * (3 + 8 + 3 + 8 * 4 + 9) * 9 + 3 + 5 + 7)",
            "2 + 3 * 5 + (3 + (2 + 2 * 6 + 9 + 7)) * ((7 * 8 + 2 * 4) * 2 * 9)",
            "7 * 4 + (8 * 3 + (5 + 9 + 8) + 7) * 2 * 2 + (8 + 7)",
            "(8 * 6) + 8 + 7 + 6 + 4",
            "((7 + 3 * 4 + 8) + 8) * 8 + 3 * ((8 + 3 + 4 + 8 * 3 + 3) + 2 + 5 + (5 + 6 + 2 * 2 * 7) * 4) + 9",
            "6 + 7 * (8 * 8 * 9) * 4 * 9 + (2 + 7 * 3)",
            "8 * 2 + 4 * (5 + (8 + 4 + 9) + (6 * 6)) + (8 * 6 * 6 * (2 * 9 * 4 + 3 * 7 * 7) * 6 + 4) + 2",
            "3 + (4 * 3 * 8 * 7 + 7 + 3) * 2 * (4 * 9 + 9) * 5",
            "3 * (5 * 6 + 3 + 9 * 2 + (6 * 6 * 9 * 6 + 2)) + 8 + 9 + ((5 + 6 * 3 + 9 * 9 + 8) + 3 * 9 + (4 + 4 * 7 * 7 + 6) + 3) * 7",
            "9 + 4 + 6 * 6 + (7 + (3 * 5)) * (8 + (3 * 4 * 4 * 4) + 2 * 8 * 7)",
            "3 + 6 * 9 * ((6 * 9 + 5 + 8) * 7 + (4 + 4) * 3 + 4) * (5 + 5)",
            "6 + 9",
            "2 + (3 + 4) * 3 * 7",
            "8 * 8 + (3 + 5 * 9 + 4 * 6 * 8) * 7 + ((5 * 5 + 3) * 9 + 5 * 8 * 5 + 6) + 9",
            "7 + (7 * 5 * (5 * 5 * 7) + 4 * 8 * 6)",
            "6 + 5 * (5 + 4 + (4 * 7)) * 5 * 7",
            "2 + (8 + 4 * 5 + 8) + 5 * 3 + ((5 * 6 + 6) + 3 + 7 + (5 * 4 * 8 + 4 * 9 * 2) + 8 + 9)",
            "(9 * 3 * 9) + (8 * 4) + 9 + 3",
            "6 + 8 + ((9 * 8 + 5 * 8 + 5) + 3 + 5 + 7 * 2 + 5)",
            "6 + 3 + ((3 * 9 * 4 * 4 + 5) * (5 * 6 * 2 * 6 * 7) * 7)",
            "5 * (7 * 8 + 3 * 4 * (7 + 3 + 3) * 7)",
            "4 + (5 * (8 + 5 * 4 + 3) * 7 * 6 + (4 + 9 * 6 + 8 + 8 + 5) * 2) + 2 + 6",
            "(9 + 7 * (8 * 3 * 5 + 6)) + (9 * 3)",
            "5 + (3 * 8 + 2 * (8 * 3 + 3 * 6) * 4 + 2) * 8",
            "6 * (3 + (5 + 8 + 9 + 9 + 3)) + 5 + 4 * 9 * 7",
            "5 + (6 * (5 + 3 + 9 * 8) * 5) * 4 * 7 * 4",
            "2 + 3 + 5 * ((9 + 4 * 3 * 4 + 8 * 2) + (7 * 5)) * (8 + 9 * 3 * 6 * 7)",
            "3 * 4 + (4 * 4) + 5 * 5",
            "2 + 5 + 5 + (2 + 2 * 2 * 2) * 9",
            "9 * 3 * 3 * 5 * 5 + (9 * 6)",
            "4 + 4 + (9 + 7 * (6 + 9 * 3) * 7) + 3 * 3 + (7 + 8 * 7 + 6 + 3)",
            "(8 + (3 + 9)) * 8 + 9 + 6 + ((5 * 8 * 7 + 3 + 2 * 7) + 5 + 9 + 6) * 2",
            "5 + 8 * ((7 * 6 + 9 * 3 + 8 + 8) + 7 + (9 * 7 + 8 * 7 * 5)) + 4 * 2",
            "7 * (4 + 4 * 3 + 6 + (9 * 9 + 7 * 3 + 8 + 5) + 7) + 7 + 6",
            "2 + 2 + 5 + 2 * 3 + 6",
            "2 + 9 * (3 + (8 * 2 * 8 * 8 * 7)) + (2 * 4) * 5",
            "5 + 2 + 7 + 4 + 9 * (7 * 4 + 4 * 5 + 9 + (7 * 2 + 4 + 6))",
            "(6 * 2 + 5 * 4) * 9 * ((3 * 2 + 3 + 9 + 9) * 4)",
            "4 + (8 * 2 * 9 + 6 + 4 + 2) + 5 + 2 + 6",
            "9 * 7 + (4 * (3 * 3 * 4 + 8 + 6) + (2 + 9 * 2) + 6) * ((9 * 5 + 4) + 6 + (3 * 2 * 7) * 6) + 8 * 5",
            "2 * (4 * 5 * 7) + 8 * 4 + 6 * 8",
            "5 * 8 * (5 + 9 * 5 + 4) + 7 + 8 + (3 + 9 * 9)",
            "(8 * 6 * 8 + 3) + 3 + 5",
            "(4 * (3 + 3 * 2 + 6 + 8) * 9 * (9 * 9) * 5) * 8 + 7 * 7 * 2 + 9",
            "4 + ((7 * 9) + 6 + 2 * 2 + 6 * 8) * 4 * 5 + 6 * 4",
            "5 * 7 * (8 * (2 + 2 + 2 + 6) + 2 + 4)",
            "5 * 4 + ((8 + 9 * 5 + 8 + 3) + 8)",
            "2 + 7 + 6 * (3 + (5 + 5 * 3) + 8 * (3 + 2 + 4)) + (4 + 6) * 6",
            "(4 + 5 + 9 * (2 * 2 + 3 + 4 * 9)) * 3 + 4 + 2",
            "3 + 7 * ((7 * 5 + 7 + 9 + 7 + 6) + 4 + 4 * 8 * 3 + (3 + 7 * 4)) + 7 + 2 + 6",
            "3 * (2 * 8 + 2 + 6) * 6 + 4 * 3",
            "5 + ((9 * 6 + 9 * 8 * 4) * 9 * 7) * 5 + 9 + 4 + 9",
            "5 * 3 * 9 * (7 * 3 * (6 * 3) * 4 + 8) + (5 + (3 + 7) + 6) * 7",
            "4 + 7",
            "(4 + 9 + 7 + (5 * 5) + 7) * 9",
            "2 * (8 * 7 + 3 * (9 + 2 * 3 * 2 + 2 * 2) * 2) * 6 + 4 * 8",
            "(8 + (4 + 6 + 9 + 3) + (3 + 7 + 3 + 8) + 8 + 2 * 5) * (6 * (8 * 9 + 3 + 6 + 3 + 5) * 3 * 4)",
            "4 + 2 * (6 * 4 * 2) * 5 + 4",
            "4 * (4 * 7) * 4 + 5 * (6 + 6 * 3 * (8 + 2 + 3 + 7 * 5)) + (2 * 9 * 9 * 9)",
            "3 * 6 * 9 + 6 + 9 * (5 * 5 * 2 + (6 + 9) * (9 * 5 * 7) + 2)",
            "4 + 8 * 8 + ((4 + 5) + 7 * 4 * 3)",
            "7 + 9 * 5 + (6 * 6 + 6) + (8 + 5)",
            "4 * (7 * 6) * 7 * 7 + ((8 * 6 * 8 * 2) * 4 * 9 + 5 + 4) + 6",
            "(7 * 2) + 6 + 5 * (5 * 7 * 6 * 7) * 9",
            "5 * 9 + 8 * (6 * 9 + 4 * 2 + 2 * 6)",
            "4 * 7 * (2 * 8 * (2 * 8 * 2 * 8 + 5 + 8) * 9) * (6 + 9 * (7 + 2 + 3 * 9)) + 8 + (2 + (6 * 2 + 8 + 8) * 4 + 8)",
            "9 * 7 * 2 + 9 + 3 * (9 + 4 + 9)",
            "3 + (5 * 4) * 8 + 9 + (6 + 8 * 8 * 5 + 8 * 7)",
            "(6 + 3 * (5 + 2 + 8 + 5 + 2 * 7)) + 2",
            "7 * ((8 * 5 * 4 * 4 * 6) + 2 + (5 + 8 + 9)) + 5 + 5 * 2",
            "2 * ((6 * 9 * 2 * 7) * 2) * 7 * 6 * 5 * 9",
            "5 + 4 + (6 + 8 + 4) * 9 + 8 * 2",
            "2 * 5 * 3 + 2 * 3 * (2 + 3 * 8 * 6)",
            "5 * (6 * 7 * 2) * (8 + (7 + 9 * 4 * 6 * 5 * 6) + 3 * 6)",
            "6 * (8 + 6 + 2 + (7 + 7 * 5 + 8) + (4 + 5 * 2 + 3 * 4 * 3) + 8) * 5 + 3 + 5",
            "(6 * 7 + (9 * 9 * 5 * 5 + 3) * 2) + 3 * 8 * 3",
            "(2 + 7) * 3 + 7 * 6",
            "(8 + 4 * 3 * (6 * 5 + 8 * 8 + 2 * 6) + 9) * (6 * (2 * 3 + 3 * 6 * 8 * 5) * 2) + 3 + 3 + 7",
            "2 + 8 * (3 + 5 * 6) * 5 * (5 + 5 * (7 * 8) + (7 + 4 + 6)) * (5 * 8 + 3)",
            "(7 + (9 * 3)) + 2 + 7 + ((4 + 4 + 8 + 7 * 6 * 2) * (7 + 3 + 5 * 4 + 2 + 4) + 4) * (4 + 7 + (6 * 4 + 4 * 7)) + ((2 * 2 + 8 * 4 + 6) + 9 * 2 + 7 + (6 + 6 * 9))",
            "6 * ((8 + 7 + 3 * 7 + 5 * 6) + 4 + 5 * (6 * 2 + 6 + 3 + 4) + 3) * 9",
            "2 * 7 + (9 * 3)",
            "5 * (5 + 3 * 4 * 9)",
            "5 + 6 * 7 * 5 * (2 + (3 + 6 * 9) + 8) + 7",
            "((7 + 8) + 3) * 2 * (8 + 8)",
            "2 * 5 * 7 * 2 + 6",
            "(5 * 2 + 4) * 8",
            "7 * 9 * 9 * (8 + 3 + 4 * 2 * (7 * 5 * 4 * 3 + 3) + 3)",
            "7 + 8 * 4 + (3 * 7)",
            "7 * 9 * (2 + 6 * 4 + 5 * 8 + (6 * 4 + 5 * 4 + 7)) * 6",
            "(7 * 8 + 8 + 8 * 8 * 4) * 5 * 2 * 2 + 2",
            "8 + (5 + 2 + 7) + 4 + (3 + 7 + 3 + 2 + 2 + 6)",
            "3 * 3 * 8 * (6 * 7 * (4 * 7 + 7 * 2 * 2 * 6)) * 7",
            "9 + (9 * 5 * (7 + 8 * 2 + 6 + 2 + 7) + 7) + (9 + (5 + 2 * 7) * (4 + 7 + 2 * 5 + 5 + 7))",
            "4 * 7 + 2 + 6",
            "3 * 7 + (9 * 2 * (8 + 2 + 3 + 4 + 9 * 6) + 5 * 2) + 9",
            "(3 * (8 + 6) * 7 + 6 * 7 + 4) + 6 * 6 * 5 * 4 * 6",
            "9 * ((3 * 4 + 7 * 6) + 3 + (2 + 5 * 7 + 3 * 9)) * 8 * 4 * 9 + 4",
            "3 * 3 * 8 * (5 + 7 * (2 + 5 * 2 * 4 * 7 * 5) + 8) + 3",
            "3 * 2 + ((8 * 7) * 4 * 8) * 3 + 6 * 3",
            "(7 + (5 + 4 * 5 + 5) + 5 + 8 + 8) + (6 + 2 + 9) + 2 + 3 * 5 + 7",
            "6 + (7 * (2 + 7 * 2 + 9 * 7) + (5 * 8 + 2)) + 7 * 7 * (9 + 3) * 3",
            "(4 * 3 * (6 + 2 * 5 + 3) + 6 * 5 + 9) * (7 * 2 * 8 + 5) * (8 + 4 * (3 * 4 + 6 + 2 + 7)) + 6 * (6 + 4)",
            "(5 * (9 + 9 * 8 + 2 + 4 + 8) * 4 + 2 * 3) + 5 * 4 * 3 + 3",
            "6 * (3 + 6 * 5 + 4 * 3 * 2) + 9",
            "((6 + 6 * 5 * 5 * 2 + 9) * 5 + 6 + 5) + 5 + (8 * 7) + 5 * 7",
            "7 + (6 + 4 + 3 * 2 + 3 + (3 + 9 + 2 + 3)) + 8 + 7 + 2",
            "7 + 8 * (6 * 2 + 4 * (6 + 9 * 8) * 8) * 9 * 2",
            "9 + (6 + 3 * 5 + (4 + 7 * 6 + 5) * 8) * 2",
            "8 + (7 + (6 + 3 + 3 + 6)) * 9",
            "(9 * 4 + 2 + 3 * (4 * 8 + 6 + 8 * 4)) * 9 + 5 + (3 * 4 * 8 + 6) * 8",
            "5 + 5 + (2 * 6 + 9) + (9 * 8 * 7 + 4) + 5",
            "(7 * 5 * 6 + 2 * 9 * 3) * (6 + 4 * 8) * 6 + 2",
            "6 * (4 + 4) + 9 + (5 * 3) * 4",
            "(9 * 6 + 8 + 3) + 3 * 4 + 8 * (7 + 9 * 9)",
            "2 * (4 * (6 * 4) + (4 * 5 + 8 * 5 + 6 * 4) * 7 + 2 + 3)",
            "(5 + 3 * (2 * 7 * 2 * 8 * 3 + 7) * (9 + 8 * 6 * 4 * 5) * 4) * 7 * 5 * (4 + 3) + 7",
            "3 + 4 * 2 + (2 * 4 + 9 + 5)",
            "((9 * 2 + 7) + 5 + (2 * 9 + 8 + 6) + 8 * 7) * 4 * 5 + (8 * 2 + 9 * 9 + (2 * 5 * 8 + 5 * 5) + 7) + (5 + (8 + 5 * 9 * 7 + 7 * 8) + 6 * 3 * 3)",
            "2 * 6 + 9 + 9 + (2 + 2 + 7 * (5 * 8 * 2 * 3 * 9)) * 2",
            "7 * 4 + 7 + 5",
            "5 + 5 + 3 * 9 + (6 * (6 * 3))",
            "5 + ((3 + 3 + 3 + 3 * 8) * 6 * (8 + 4 + 8 + 3 * 7) + 3 * (4 + 8 + 3 + 6 + 6 + 5) * 4) + 2 * 6",
            "(2 * 6 + 4 + (8 + 3 * 7 * 8 * 7) * 7 + 7) + 3 + 8 * 7 + 4 * (9 * 9 + 7 + 9 * 7)",
            "8 + (9 * 8 + 2) * ((8 * 5 * 8 * 2 * 2 * 6) + 6 * 5) + 3 * 7 * (5 + 2)",
            "5 + ((7 + 6 + 5) + 8 * 9 + 7 * (7 + 8 * 8 + 8 + 3)) + (3 + 6)",
            "5 * 7 + 2 + (5 + 8 + 3 * 2 * 5) * 8 + ((4 + 4) + (9 * 7 * 9 + 4) * 9 + (3 * 9 * 3 + 3 * 9) * 5 * 6)",
            "(9 + 5 + 3) + 8 + 4 * 8 + 3 + 7",
            "6 + 6 * 5 * (9 * 5 + 5)",
            "5 + ((7 * 2) + 9 + 9)",
            "2 + (5 * (6 * 8 * 5 * 6 * 9 + 6) * 4) + 8 * 4 * 8",
            "(3 + 7 + 2 + (2 + 3 * 9 + 5) * 2 + (4 + 4 + 3 * 8 + 7)) * 9 * 8 + 2",
            "9 * 2 * ((5 + 2 * 7) + 9 + 6 + 8) + 5 * 7",
            "9 + 6 * 6 * 8 + 7 * 4",
            "7 * 8 * (2 + 4) * 3",
            "5 + 3 + 6 + ((6 * 2 * 4 + 5 * 6 * 6) * 5) + 5",
            "((8 + 2 * 4) + 8 * (8 + 2 * 6 + 3 * 8 * 6) + 8 + 6 * 9) + 2 + 4",
            "7 + (8 + 8) * 4 + (8 + (9 * 3 * 6 * 5) + 5 + (4 + 2)) * 4",
            "((6 + 9) + 6 + 5) * (7 + (9 * 7 * 6 * 8) + (8 + 7 * 4 * 8 * 5 * 4) * 2 + 5) * 2 + 5",
            "4 + 8 + (8 * 8 + 6 * 3 + 7) * 5",
            "6 + 9",
            "(5 * (4 + 6 * 2)) + (9 + 9 + 6 * 4 + 3) * 4 + 4 + 2",
            "((7 + 3 + 8) * 9) + 9 * 2 * 3 * 8",
            "(7 * (5 * 8 + 6 + 2) + 5 * 6 + (8 + 3)) * 4 * 7 + (2 + 9 + 2)",
            "7 + 5 * ((5 * 4 + 2 + 9) * 9)",
            "(3 + (7 + 7 * 6 * 2 + 9)) + (5 * 6 + 4 * 5 * 7 + 2) * 2 * 3 * 3 + 9",
            "(5 + 7 * 8 + 5 + 4) + 8 * 7",
            "(3 * 2 + 7) + 6 + (4 * 5 * 4 * (3 + 9 * 3)) * (6 + 2 * 9 * (3 + 5 + 7 * 7)) * (7 * 6)",
            "8 * 5 + 2 + (5 * 7 + 2 + 5 * (7 + 7 + 4 * 8 + 3 * 5)) * 8 + (7 + 3)",
            "6 * 2 * 2 + ((7 + 5 + 7 * 7) * 2 + 7 + 2 + 9) * 2 * (5 + (7 + 4) + 3)",
            "7 * 9 + (7 * 6 + (5 * 2) * 3 + 5 * (8 * 9 * 4)) + (6 * 3)",
            "3 * 3 + 4 * 8",
            "((2 + 8 * 6 + 2) * 4) * ((4 + 4 + 5 + 4 * 8) * 5 * 7 * 6) * 2",
            "7 * (4 * 7 + (5 * 5 * 7 + 2 + 4) + 4 * 5 + 4) * (2 + 6 * 2 * 4) * 2 + 5 * 7",
            "4 * 3 * 8 * 3 + 2",
            "(8 * 7 * (3 + 8) + (6 + 4 * 9 + 2 + 9 * 7) * (3 * 9 * 3 + 7 * 7) + 9) + 3",
            "(9 + 2 * (7 * 5 + 9 + 4 * 4 + 2)) + 3 + 2 * 7 * 9",
            "4 + 3 + 5 * (3 + 6 + (2 + 9 * 8 * 8 * 5 + 9) + 2)",
            "4 * (9 * 8 + 2) * 2 + 8 + 5",
            "9 * 7 * 4 * (6 * 2 + 7) * 7 + 7",
            "4 * 9 * 5 + (3 * 2 + 5 + 7) + 8 + 8",
            "4 + 3 * (7 * (3 + 8 * 2 + 8 + 8 + 4) * 2)",
            "(4 * 2) + (2 + 7 + (5 + 2) * 9 * 8 * 7)",
            "(7 + 8) + 3 + 5 + 3 * 5 + 9",
            "(4 * 8 * 2 * 8) + 5 * ((6 + 3 + 2 * 4 + 2) * 2 * 4 + 2) + 9",
            "3 + 3 * ((4 + 6 + 4 + 7) + (9 + 8) * (5 + 9 * 3 + 4 + 7) + 9) * 8 + (8 + 6 * (6 * 6 * 5) + 8 + 5) * (8 + 7)",
            "8 * (5 + (4 + 8 * 5 + 8 + 9 * 7) * 2) + 6 * 5 + 4 * (5 * 9 * 2 * 5 * (2 + 3 * 8 * 7) * (4 * 2))",
            "4 + (9 * (8 * 8 + 3 * 3 * 5) + 5 + 3 + (4 * 8)) * 2 + (3 * 4 * 9 + 7) + 8 * 9",
            "2 + 2 * 7 * (9 + (4 * 9 * 4 * 8 + 3 * 9) + 4) + 3 + 9",
            "2 * 3 * 2 * 7 * (5 + (7 + 8 * 2) + (5 * 7 * 8 * 2 + 8) + (2 * 4 + 9) + 2 + 8)",
            "4 * (8 + 6 * 3) * 8",
            "((6 * 9) * (9 * 7 + 3) * 5 + 7) + 8 * 8 * 8 * (5 + 7 * (2 + 9))",
            "6 + 9 * ((8 * 8 + 9 + 9) * 3 * 4) * 3",
            "(7 + 6 + 7 + (4 + 7 * 7 * 6) + (4 + 8) + (9 + 7 * 7 * 9 * 8)) * 5 * 7 + 6 + (3 + (9 + 5 * 2) * 6 + 6)",
            "6 + (8 * 7 + 4 * 2 * 8) + 9 + 7 * 6",
            "5 * 5 * 5 + 9 * 4 * 2",
            "4 + 9 * 9 + ((5 + 3 + 2 * 4) * 6)",
            "2 + (6 * 4 * 5 * 4 * 2 + 6) * (5 * 8)",
            "6 * (7 * 4 + 2 * (3 + 9 * 2 + 6 + 9) + 8 * 7) + (2 * 7) + 2 * 6",
            "(2 + 9) + 8 + (7 * 9 + 7) * 7 * (3 + (2 * 9 * 4 * 5) * 5)",
            "3 * 4 * 5 * (9 + 9 * 6 + 4 + 4 * (8 * 7 + 6)) + 9 * 7",
            "(7 * (9 + 8 * 9 + 5) + 9 + (8 + 4) + 5) + 7 + 6 + 2",
            "9 + 2 + (5 * 5 + 7)",
            "(6 * 5 + (8 * 8 * 9 + 3 * 9)) + 2",
            "(4 + 2 * 7 * 2 * 7) * (6 + 2 + 4) + 6",
            "3 * 6 + 8 + (4 + (4 * 2 + 8) * (9 + 4 + 2)) * 2 + (6 + (9 * 4 + 3 * 6 * 6 * 2) * 9 + 6 + 8 * 7)",
            "(5 + (6 * 2 + 4) + 8 * 8) + 4 * (3 * 8 + 9 + 5 * 8 + 9) + 5",
            "6 + 7 + (4 + 5 * 3 + (4 * 7 * 2 + 4 * 3) + 3)",
            "3 + 8 * (6 + 6 + 9 * (7 * 7 * 2 * 6 + 9)) + 7 * 9",
            "7 * 5 * 9 * 9 + (2 * 8 * 9 + 4 + (9 + 3 * 5 + 4 + 6 + 8) * 2) + 6",
            "((5 * 4 + 4 * 2 + 4) + 3 * 6 + 6) + ((9 + 4 * 4 * 9) * 3 * 4)",
            "5 * 6 + 9 * 9 * 5",
            "4 + (7 + 8 + 6 * 2 + 6) + 6 + 2 * 9 * 9",
            "(5 + (6 * 7 * 8 * 9) + 7 + 9 + 9) * (7 + 9 + 7 * 3 + 4) + 8 * 5 + (3 + (4 * 8) * 4 * 7)",
            "3 + (7 * 8 * 5 * 6 * (3 + 8) * 3) * 4",
            "8 + (5 + 5 * (9 * 5 * 3) * 7 + 8) + 6 * 2",
            "9 + (9 + 2 + (5 + 2 * 3 * 6 + 4) + 7 * 8) * (5 * 4 + (9 + 5 + 2 + 3 + 2 + 2) * 7 * (5 * 9 * 6 * 3) + 3) + 5",
            "(7 * 6 * 7 + 8) + (5 + 8 + 2) * 9",
            "3 + 7 * 2 * (8 * (8 + 4 * 6 * 9) * 3 * 9) * (6 + (5 * 8) * 5 * 3 + 4) + 3",
            "8 * 3 * (7 * 5 + (8 * 7 * 9 * 4 * 3) * 3 + 2) * 8",
            "5 * 5 * (9 * (9 * 7)) + 2",
            "(6 * 7 + 6 * 6 * 5) + 7",
            "7 + 8 * 3 + ((2 * 2 + 9 * 3 * 7 + 2) + 4 * 8 + (7 * 8 + 4)) + 5",
            "7 * 2 * ((6 * 8 * 6 + 4 * 5 * 2) * 2 + (5 * 2 * 2 + 2 * 5 * 5) + 5)",
            "9 + 4 + 7 * (6 + 4) * 4",
            "((8 + 2 * 4 * 6) + 4 + 5 + 9) + (8 + 4 * 7 + 6)",
            "((8 + 9 * 4 * 2 * 8 + 9) + (2 * 7 * 2 + 8 + 7) * 9 + 6 + (4 * 3 * 2 + 4 * 9) * 7) * 8 + 4 + 4",
            "4 + (8 * 2 + 9 + 5) * 6 * 5",
            "7 + 7 + (9 * (5 + 3 * 2 * 9 * 9 * 7) + 4 * 4 * 2) * (9 * 8)",
            "(2 * 5 + 4 + 9 * 5 + 3) + 6 * 6 + 6 + 4 * (4 * 8)",
            "(4 + (9 + 2) * (4 + 3 + 7 * 6 * 9) + 7) + 2 + 6 * 3 + 4 + 5",
            "9 + 6 * (8 * (7 * 4 * 7 + 5 * 9 * 2))",
            "(2 + 5 + 9 + (9 + 2)) + 5 + 6",
            "3 + 5 + 5 + 2 + 9 * ((7 + 6) * 2)",
            "(5 + 2 * 6 * 6 * 5 * 3) * 9 + 7 + (6 + 6 * 4 + 8 + 4 * 4) + 2 * 2",
            "(8 + (7 * 2 * 2 * 3 + 6 * 6)) * 7 * 8",
            "8 + ((8 * 9 * 8 * 5 + 7 + 5) * 5 + 7 * (8 * 7) * 8) + ((3 * 7 * 8 + 9 + 4) + 6 * 7 * 2 + 8 * (7 + 2)) + 7",
            "(8 + 9 * 4 * 6 * 6 * 7) + 7 * 3 * 7 + (7 + 5) + 8",
            "7 * 7 + (6 * 6 + (7 * 4 + 9) + (2 * 4 + 7)) + 8",
            "(8 + (8 + 9 * 2 * 6) + 5) * 2 + 8",
            "8 + 2 + 5 * 7",
            "(9 + (4 + 8 + 3 + 6 * 3)) * 7 + ((4 * 8 + 3 * 7 * 5 * 9) + 4) * ((5 * 9 * 5 + 6) * (6 + 2 + 8) * (7 * 8 * 8) + 9 * 7 + 9) + 2",
            "5 + 7 * 7 * 3 + (5 * 2)",
            "9 + 8 + 9 + (7 + (9 * 6 * 5 * 4 * 7 + 5) + 7 + 7 + 9 + (6 * 3 * 2 + 3 + 9))",
            "(6 * (9 * 3 * 6)) + 3 * 9 * 8 + (3 + (6 * 6 * 5 * 8 + 9) + 7 * 5)",
            "2 + 9 + ((4 + 4 * 9 * 6) * (8 + 6 + 6 + 2)) * 2 + 4",
            "3 + 5 + ((4 + 9) * 7 * 6)",
            "7 * 2 * 7 * 9 * (7 * (7 * 6 + 2) * (6 + 8 + 9 + 9) + (3 + 7 + 5 * 2) + 2) * (9 * (8 + 9 + 8 + 6 * 2 + 2) + 5 * (3 + 8 + 7 * 4))",
            "4 + (3 * 2 + (5 * 4 + 9 * 4 * 8) * (5 * 3 + 6)) * 6 + 7 + 8 * 3",
            "2 + 5 * (7 * 3 * 6 * 4 * 8 + 9)",
            "7 + ((2 + 4 + 4 + 6) + 6 + 8) * 6 + 6 + 5",
            "9 * 4 + 4",
            "(3 + 3 * 3 * 8 * 6 * 4) + 6 + 3",
            "5 + 8 + 7",
            "4 * (4 + (9 + 5) * 5 * 7 * 3 * 2) * 8 * 7",
            "5 * ((5 * 7 * 5 + 9 + 9 + 2) + 6 * 7 * 9)",
            "6 * 5 + 9 + 8 + 3 * 5",
            "9 + 4",
            "4 + 5 + 7 + 4 * 8 * (8 * 3 * 6 * 6 + (3 + 4 + 4 * 3 + 7) * 2)",
            "4 + 7 * (9 * 8 + 2) + (3 + 7)",
            "(3 * 4 + 5 + 5 * 3) + 9 * 3 + 2 + 4",
            "(3 * 3 * 4 + 6) + 2 * 4 * 2 * 2",
            "(7 * 7 + 9 * 3 + 9) + 2 * 6 * 7",
            "9 + 6 + 6 + 7 + 2 + ((9 + 3 * 3 * 6 + 2) * (4 + 5) + 8 + 3 + 3 * 9)",
            "8 * (2 * (9 * 2 * 9 + 4 + 3) * (5 * 3 + 3 + 9) + 6 * 3) + 3",
            "8 * 2 + 8 * ((4 * 5) * 8 * 5 * 5 * (3 + 4 + 7 * 9 + 5 + 4) + 2) * 8",
            "(9 * 3 + 4 + (2 * 6 + 3 * 9 * 9 + 6)) * 3 + 8",
            "3 + (7 * 5 * 4)",
            "((3 + 2 * 3 * 3 * 9) + 4 * 4) * 3",
            "5 * (9 * 7 * 8) + (8 * 9 * 8)",
            "((6 * 2 * 3) * 6 * 3 + 9) * 8 + 3 * 7 * 5 * 2"
        };
    }
}