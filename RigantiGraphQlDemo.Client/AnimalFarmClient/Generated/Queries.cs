using StrawberryShake;
using System;

namespace RigantiGraphQlDemo.Client
{
    public class Queries
        : IDocument
    {
        private readonly byte[] _hashName = {
            109,
            100,
            53,
            72,
            97,
            115,
            104
        };
        private readonly byte[] _hash = {
            117,
            87,
            74,
            85,
            70,
            103,
            109,
            113,
            86,
            49,
            109,
            88,
            83,
            56,
            107,
            116,
            48,
            110,
            69,
            84,
            74,
            103,
            61,
            61
        };
        private readonly byte[] _content = {
            113,
            117,
            101,
            114,
            121,
            32,
            102,
            97,
            114,
            109,
            76,
            105,
            115,
            116,
            32,
            123,
            32,
            102,
            97,
            114,
            109,
            115,
            32,
            123,
            32,
            95,
            95,
            116,
            121,
            112,
            101,
            110,
            97,
            109,
            101,
            32,
            110,
            111,
            100,
            101,
            115,
            32,
            123,
            32,
            95,
            95,
            116,
            121,
            112,
            101,
            110,
            97,
            109,
            101,
            32,
            105,
            100,
            32,
            110,
            97,
            109,
            101,
            32,
            125,
            32,
            125,
            32,
            125,
            32,
            113,
            117,
            101,
            114,
            121,
            32,
            97,
            110,
            105,
            109,
            97,
            108,
            76,
            105,
            115,
            116,
            32,
            123,
            32,
            97,
            110,
            105,
            109,
            97,
            108,
            115,
            32,
            123,
            32,
            95,
            95,
            116,
            121,
            112,
            101,
            110,
            97,
            109,
            101,
            32,
            110,
            111,
            100,
            101,
            115,
            32,
            123,
            32,
            95,
            95,
            116,
            121,
            112,
            101,
            110,
            97,
            109,
            101,
            32,
            105,
            100,
            32,
            110,
            97,
            109,
            101,
            32,
            115,
            112,
            101,
            99,
            105,
            101,
            115,
            32,
            102,
            97,
            114,
            109,
            73,
            100,
            32,
            125,
            32,
            125,
            32,
            125
        };

        public ReadOnlySpan<byte> HashName => _hashName;

        public ReadOnlySpan<byte> Hash => _hash;

        public ReadOnlySpan<byte> Content => _content;

        public static Queries Default { get; } = new();

        public override string ToString() => 
            @"query farmList {
              farms {
                nodes {
                  id
                  name
                }
              }
            }
            
            query animalList {
              animals {
                nodes {
                  id
                  name
                  species
                  farmId
                }
              }
            }";
    }
}
