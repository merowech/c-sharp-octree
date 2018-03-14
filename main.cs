using System;
using System.Collections.Generic;

namespace CSharpOctree {

    public class Test
    {
        static public void Main(string[] args)
        {
            double min = 0.0d, max = 100.0d;
            int limit = 100000;

            Octree oc = new Octree(min, max, min, max, min, max, 5);

            Random random = new Random();
            for (int i = 0; i < limit; i++)
            {
                double x = random.NextDouble() * (max - min) + min;
                double y = random.NextDouble() * (max - min) + min;
                double z = random.NextDouble() * (max - min) + min;
                Console.WriteLine("Added " + i + " - " + x + " " + y + " " + z);
                oc.Add(x, y, z, i);
            }

            Console.WriteLine(oc.printTree());

            Console.WriteLine("Depth: " + oc.getDepth());
            Console.WriteLine("Outer Nodes: " + oc.getNumberOfOuterNodes());
            Console.WriteLine("Representations: " + oc.getRepresentation().Count);
        }
    }
}
